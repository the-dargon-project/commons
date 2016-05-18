using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using static Dargon.Commons.Channels.ToFuncTTaskConverter;

namespace Dargon.Commons.Channels {
   public class DispatchContext {
      public const int kTimesInfinite = int.MinValue;

      private readonly CancellationTokenSource cts = new CancellationTokenSource();
      private readonly AsyncManualResetEvent completionLatch = new AsyncManualResetEvent(false);
      private readonly ConcurrentQueue<Task> tasksToShutdown = new ConcurrentQueue<Task>();
      private int dispatchesRemaining;

      public DispatchContext(int times) {
         dispatchesRemaining = times;
      }

      public bool IsCompleted => cts.IsCancellationRequested;

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
         while (!cts.IsCancellationRequested) {
            bool isFinalDispatch = false;
            var result = await channel.ReadAsync(
               cts.Token,
               acceptanceTest => {
                  if (dispatchesRemaining == kTimesInfinite) {
                     return true;
                  } else {
                     var oldDispatchesRemaining = Interlocked.Decrement(ref dispatchesRemaining) + 1;
                     isFinalDispatch = oldDispatchesRemaining == 1;
                     return oldDispatchesRemaining > 0;
                  }
               }).ConfigureAwait(false);
            if (isFinalDispatch) {
               cts.Cancel();
               await callback(result).ConfigureAwait(false);
               completionLatch.Set();
            } else {
               await callback(result).ConfigureAwait(false);
            }
         }
      }
      
      public async Task WaitAsync(CancellationToken token = default(CancellationToken)) {
         await Task.WhenAny(completionLatch.WaitAsync(), token.AsTask())
                   .ConfigureAwait(false);
      }

      public async Task ShutdownAsync() {
         cts.Cancel();
         completionLatch.Set();
         foreach (var task in tasksToShutdown) {
            try {
               await task;
            } catch (TaskCanceledException) {
               // okay
            }
         }
      }
   }
}