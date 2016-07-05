using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   /// <summary>
   /// Not-so-amazingly-performant (compared to a bool branch) variant
   /// of an awaitable once-latch that supports wait cancellation.
   /// </summary>
   public class AsyncLatch {
      private readonly TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(TaskContinuationOptions.RunContinuationsAsynchronously);
      private const int kStateUnsignalled = 0;
      private const int kStateSignalled = 1;
      private int state = kStateUnsignalled;

      public async Task WaitAsync(CancellationToken token = default(CancellationToken)) {
         await Task.WhenAny(token.AsTask(), tcs.Task).ConfigureAwait(false);
         token.ThrowIfCancellationRequested();
      }

      public void Set() {
         if (Interlocked.CompareExchange(ref state, kStateSignalled, kStateUnsignalled) == kStateUnsignalled) {
            tcs.SetResult(false);
         }
      }
   }
}