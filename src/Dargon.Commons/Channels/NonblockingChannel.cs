using Nito.AsyncEx;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public class NonblockingChannel<T> : Channel<T> {
      private readonly ConcurrentQueue<T> writeQueue;
      private readonly AsyncSemaphore readSemaphore;
      private readonly AsyncSemaphore writeSemaphore;

      public NonblockingChannel(ConcurrentQueue<T> writeQueue, AsyncSemaphore readSemaphore, AsyncSemaphore writeSemaphore) {
         this.writeQueue = writeQueue;
         this.readSemaphore = readSemaphore;
         this.writeSemaphore = writeSemaphore;
      }

      public async Task WriteAsync(T message, CancellationToken cancellationToken) {
         if (writeSemaphore != null) {
            await writeSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
         }
         writeQueue.Enqueue(message);
         readSemaphore.Release();
      }

      public async Task<T> ReadAsync(CancellationToken cancellationToken, Func<T, bool> acceptanceTest) {
         await Task.Yield();
         while (!cancellationToken.IsCancellationRequested) {
            await readSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            T message;
            while (!writeQueue.TryDequeue(out message)) { }
            if (acceptanceTest(message)) {
               writeSemaphore?.Release();
               return message;
            } else {
               writeQueue.Enqueue(message);
               readSemaphore.Release();
            }
         }
         // throw is guaranteed
         cancellationToken.ThrowIfCancellationRequested();
         return default(T);
      }
   }
}