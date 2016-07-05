using System;
using System.Runtime.Remoting.Contexts;
using Dargon.Commons.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncReaderWriterLock {
      private readonly AsyncSemaphore semaphore = new AsyncSemaphore(1);
      private readonly ConcurrentQueue<AsyncLatch> readerQueue = new ConcurrentQueue<AsyncLatch>();
      private int pendingReadersCount = 0;
      private int readerCount = 0;

      public async Task<IDisposable> WriterLockAsync() {
         await semaphore.WaitAsync().ConfigureAwait(false);
         return new DecrementOnDisposeAndReleaseOnCallbackZeroResult(
            () => 0,
            semaphore);
      }

      public Task<IDisposable> ReaderLockAsync() {
         var spinner = new SpinWait();
         while (true) {
            var originalReaderCount = readerCount;
            var nextReaderCount = originalReaderCount + 1;
            if (Interlocked.CompareExchange(ref readerCount, nextReaderCount, originalReaderCount) == originalReaderCount) {
               if (originalReaderCount == 0) {
                  return ReaderLockCoordinatorRoleAsync();
               } else {
                  return ReaderLockFollowerRoleAsync();
               }
            }
            spinner.SpinOnce();
         }
      }

      private async Task<IDisposable> ReaderLockCoordinatorRoleAsync() {
         await semaphore.WaitAsync().ConfigureAwait(false);

         int allReadersCount = 0;
         while (true) {
            var existingReaderCount = readerCount;
            allReadersCount += existingReaderCount;
            if (Interlocked.Add(ref readerCount, -existingReaderCount) == 0) {
               break;
            }
         }

         pendingReadersCount = allReadersCount;

         var spinner = new SpinWait();
         for (var i = 0; i < allReadersCount - 1; i++) {
            AsyncLatch followerLatch;
            while (!readerQueue.TryDequeue(out followerLatch)) {
               spinner.SpinOnce();
            }
            followerLatch.Set();
         }
         return new DecrementOnDisposeAndReleaseOnCallbackZeroResult(
            () => Interlocked.Decrement(ref pendingReadersCount),
            semaphore);
      }

      private async Task<IDisposable> ReaderLockFollowerRoleAsync() {
         var latch = new AsyncLatch();
         readerQueue.Enqueue(latch);
         await latch.WaitAsync(CancellationToken.None).ConfigureAwait(false);
         return new DecrementOnDisposeAndReleaseOnCallbackZeroResult(
            () => Interlocked.Decrement(ref pendingReadersCount),
            semaphore);
      }

      private class DecrementOnDisposeAndReleaseOnCallbackZeroResult : IDisposable {
         private readonly Func<int> callback;
         private readonly AsyncSemaphore semaphore;

         public DecrementOnDisposeAndReleaseOnCallbackZeroResult(Func<int> callback, AsyncSemaphore semaphore) {
            this.callback = callback;
            this.semaphore = semaphore;
         }

         public void Dispose() {
            if (callback() == 0) {
               semaphore.Release();
            }
         }
      }
   }
}
