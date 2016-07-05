using Dargon.Commons.Collections;
using Dargon.Commons.Exceptions;
using Dargon.Commons.Pooling;
using System;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncProcessor<TInput, TPassed> {
      private readonly IObjectPool<AsyncState> asyncStatePool = ObjectPool.CreateStackBacked(() => new AsyncState());
      private readonly IConcurrentQueue<AsyncState> queue = new ConcurrentQueue<AsyncState>();
      private readonly AsyncSemaphore semaphore = new AsyncSemaphore(0);
      private readonly AsyncRouter<TInput, TPassed> router;

      public AsyncProcessor(AsyncRouter<TInput, TPassed> router) {
         this.router = router;
      }

      public void Initialize() {
         RunProcessLoopAsync().Forget();
      }

      public async Task ProcessAsync(TInput input) {
         var state = asyncStatePool.TakeObject();
         state.Input = input;
         queue.Enqueue(state);
         semaphore.Release();

         await state.CompletionLatch.WaitAsync().ConfigureAwait(false);

         state.Input = default(TInput);
         asyncStatePool.ReturnObject(state);
      }

      private async Task RunProcessLoopAsync() {
         while (true) {
            await semaphore.WaitAsync().ConfigureAwait(false);
            AsyncState state;
            if (!queue.TryDequeue(out state)) {
               throw new InvalidStateException();
            }
            try {
               await router.TryRouteAsync(state.Input).ConfigureAwait(false);
            } catch (Exception e) {
               await HandleExceptionAsync(state.Input, e).ConfigureAwait(false);
            } finally {
               state.CompletionLatch.Set();
            }
         }
      }

      protected virtual async Task HandleExceptionAsync(object input, Exception e) {
         await Console.Error.WriteAsync(e.ToString()).ConfigureAwait(false);
      }

      private class AsyncState {
         public TInput Input { get; set; }
         public AsyncLatch CompletionLatch { get; } = new AsyncLatch();
      }
   }
}