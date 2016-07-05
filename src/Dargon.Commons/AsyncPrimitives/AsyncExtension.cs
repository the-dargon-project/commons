using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public static class AsyncExtension {
      public static Task AsTask(this CancellationToken cancellationToken) {
         var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
         cancellationToken.Register(CancellationCallback, tcs, false);
         return tcs.Task;
      }

      private static void CancellationCallback(object state) {
         var tcs = (TaskCompletionSource<bool>)state;
         tcs.SetException(new TaskCanceledException());
      }
   }
}
