using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncLock {
      private readonly AsyncSemaphore semaphore = new AsyncSemaphore(1);

      public async Task<IDisposable> LockAsync(CancellationToken cancellationToken = default(CancellationToken)) {
         await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
         return new ReleaseSemaphoreOnDisposal(semaphore);
      }

      public class ReleaseSemaphoreOnDisposal : IDisposable {
         private readonly AsyncCountdownLatch duplicateFreeCheckLatch = new AsyncCountdownLatch(1);
         private readonly AsyncSemaphore semaphore;

         public ReleaseSemaphoreOnDisposal(AsyncSemaphore semaphore) {
            this.semaphore = semaphore;
         }

         public void Dispose() {
            duplicateFreeCheckLatch.Signal();
            semaphore.Release();
         }
      }
   }
}
