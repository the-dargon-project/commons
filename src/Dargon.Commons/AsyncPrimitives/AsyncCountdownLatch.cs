using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Commons.Collections;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncCountdownLatch {
      private readonly AsyncLatch latch = new AsyncLatch();
      private int count;

      public AsyncCountdownLatch(int count) {
         this.count = count;
      }

      public Task WaitAsync(CancellationToken cancellationToken = default(CancellationToken)) {
         return latch.WaitAsync(cancellationToken);
      }

      public bool Signal() {
         var decrementResult = Interlocked.Decrement(ref count);
         if (decrementResult == 0) {
            latch.Set();
            return true;
         }
         if (decrementResult < 0) {
            throw new InvalidOperationException("Attempted to decrement latch beyond zero count.");
         }
         return false;
      }
   }
}
