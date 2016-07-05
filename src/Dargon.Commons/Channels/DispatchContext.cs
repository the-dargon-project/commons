using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Commons.AsyncPrimitives;
using static Dargon.Commons.Channels.ToFuncTTaskConverter;

namespace Dargon.Commons.Channels {
   public class DispatchContext {
      public const int kTimesInfinite = int.MinValue;

      private readonly CancellationTokenSource cts = new CancellationTokenSource();
      private readonly AsyncLatch completionLatch = new AsyncLatch();
      private readonly ConcurrentQueue<Task> tasksToShutdown = new ConcurrentQueue<Task>();
      private int dispatchesRemaining;
      private bool isCompleted = false;

      public DispatchContext(int times) {
         dispatchesRemaining = times;
      }

      public bool IsCompleted => isCompleted;

      public DispatchContext Case<T>(ReadableChannel<T> channel, Action callback) {
         return Case(channel, Convert<T>(callback));
      }

      public DispatchContext Case<T>(ReadableChannel<T> channel, Action<T> callback) {
         return Case(channel, Convert<T>(callback));
      }

      public DispatchContext Case<T>(ReadableChannel<T> channel, Func<Task> callback) {
         return Case(channel, Convert<T>(callback));
      }

      public DispatchContext Case<T>(ReadableChannel<T> channel, Func<T, Task> callback) {
         var task = ProcessCaseAsync<T>(channel, callback);
         tasksToShutdown.Enqueue(task);
         return this;
      }

      private async Task ProcessCaseAsync<T>(ReadableChannel<T> channel, Func<T, Task> callback) {
         try {
            while (!cts.IsCancellationRequested) {
               bool isFinalDispatch = false;
               var result = await channel.ReadAsync(
                  cts.Token,
                  acceptanceTest => {
                     if (Interlocked.CompareExchange(ref dispatchesRemaining, 0, 0) == kTimesInfinite) {
                        return true;
                     } else {
                        var spinner = new SpinWait();
                        while (true) {
                           var capturedDispatchesRemaining = Interlocked.CompareExchange(ref dispatchesRemaining, 0, 0);
                           var nextDispatchesRemaining = capturedDispatchesRemaining - 1;

                           if (nextDispatchesRemaining < 0) {
                              return false;
                           }

                           if (Interlocked.CompareExchange(ref dispatchesRemaining, nextDispatchesRemaining, capturedDispatchesRemaining) == capturedDispatchesRemaining) {
                              isFinalDispatch = nextDispatchesRemaining == 0;
                              return true;
                           }
                           spinner.SpinOnce();
                        }
                     }
                  }).ConfigureAwait(false);
//               Console.WriteLine("Got from ch " + result);
               if (isFinalDispatch) {
                  cts.Cancel();
                  await callback(result).ConfigureAwait(false);
                  completionLatch.Set();
                  isCompleted = true;
               } else {
                  await callback(result).ConfigureAwait(false);
               }
            }
         } catch (OperationCanceledException) {
            // do nothing
         } catch (Exception e) {
            Console.Error.WriteLine(e);
            throw;
         }
      }
      
      public async Task WaitAsync(CancellationToken token = default(CancellationToken)) {
         await completionLatch.WaitAsync(token).ConfigureAwait(false);
      }

      public async Task ShutdownAsync() {
         cts.Cancel();
         completionLatch.Set();
         foreach (var task in tasksToShutdown) {
            try {
               await task.ConfigureAwait(false);
            } catch (TaskCanceledException) {
               // okay
            }
         }
      }
   }
}