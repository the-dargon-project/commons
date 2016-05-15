using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

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

      public DispatchContext Case<T>(ReadableChannel<T> channel, Func<T, Task> callback) {
         return Case(channel, new Action<T>(t => callback(t)));
      }

      public DispatchContext Case<T>(ReadableChannel<T> channel, Action<T> callback) {
         var task = ProcessCaseAsync<T>(channel, callback);
         tasksToShutdown.Enqueue(task);
         return this;
      }

      private async Task ProcessCaseAsync<T>(ReadableChannel<T> channel, Action<T> callback) {
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

               });
            if (isFinalDispatch) {
               cts.Cancel();
               callback(result);
               completionLatch.Set();
            } else {
               callback(result);
            }
         }
      }

      public Task WaitAsync() => WaitAsync(CancellationToken.None);

      public Task WaitAsync(CancellationToken token) {
         return Task.WhenAny(completionLatch.WaitAsync(), token.AsTask());
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