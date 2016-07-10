using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public static class AsyncExtension {
      private static readonly TaskCompletionSource<bool> kNeverTcs = new TaskCompletionSource<bool>();
      private static readonly Task kNeverTask = kNeverTcs.Task;

      public static Task AsTask(this CancellationToken cancellationToken) {
         if (!cancellationToken.CanBeCanceled) {
            return kNeverTask;
         } else if(cancellationToken.IsCancellationRequested) {
            return Task.CompletedTask;
         } else {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(CancellationCallback, tcs, false);
            return tcs.Task;
         }
      }

      private static void CancellationCallback(object state) {
         var tcs = (TaskCompletionSource<bool>)state;
         tcs.SetException(new TaskCanceledException());
      }
   }
}
