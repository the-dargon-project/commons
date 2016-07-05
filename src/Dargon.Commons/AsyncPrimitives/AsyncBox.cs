using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncBox<T> {
      private readonly AsyncLatch completionLatch = new AsyncLatch();
      private T result;

      public void SetResult(T value) {
         result = value;
         completionLatch.Set();
      }

      public async Task<T> GetResultAsync(CancellationToken cancellationToken = default(CancellationToken)) {
         await completionLatch.WaitAsync(cancellationToken).ConfigureAwait(false);
         return result;
      }
   }
}
