using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Dargon.Commons.Channels {
   public class BlockingChannel<T> : Channel<T> {
      private readonly ConcurrentQueue<WriterContext<T>> writerQueue = new ConcurrentQueue<WriterContext<T>>();
      private readonly AsyncSemaphore queueSemaphore = new AsyncSemaphore(0);

      public async Task WriteAsync(T message, CancellationToken cancellationToken) {
         var context = new WriterContext<T>(message);
         writerQueue.Enqueue(context);
         queueSemaphore.Release();
         try {
            await Task.WhenAny(context.resetEvent.WaitAsync(), cancellationToken.AsTask());
         } catch (OperationCanceledException) {
            while (context.state != WriterContext<T>.kStateCancelled) {
               var originalValue = Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCancelled, WriterContext<T>.kStatePending);
               if (originalValue == WriterContext<T>.kStatePending) {
                  throw;
               } else if (originalValue == WriterContext<T>.kStateCompleting) {
                  await context.completingFreedEvent.WaitAsync(CancellationToken.None);
               }
            }
         } finally {
            Trace.Assert(context.state == WriterContext<T>.kStateCancelled ||
                         context.state == WriterContext<T>.kStateCompleted);
         }
      }

      public async Task<T> ReadAsync(CancellationToken cancellationToken, Func<T, bool> acceptanceTest) {
         await Task.Yield();
         while (!cancellationToken.IsCancellationRequested) {
            await queueSemaphore.WaitAsync(cancellationToken);
            WriterContext<T> context;
            while (!writerQueue.TryDequeue(out context)) { }
            var oldState = Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleting, WriterContext<T>.kStatePending);
            if (oldState == WriterContext<T>.kStatePending) {
               if (acceptanceTest(context.value)) {
                  Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleted, WriterContext<T>.kStateCompleting);
                  context.completingFreedEvent.Set();
                  context.resetEvent.Set();
                  return context.value;
               } else {
                  Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStatePending, WriterContext<T>.kStateCompleting);
                  context.completingFreedEvent.Set();
                  writerQueue.Enqueue(context);
                  queueSemaphore.Release();
               }
            }
         }
         // throw is guaranteed
         cancellationToken.ThrowIfCancellationRequested();
         return default(T);
      }

      private class WriterContext<T> {
         public const int kStatePending = 0;
         public const int kStateCompleting = 1;
         public const int kStateCompleted = 2;
         public const int kStateCancelled = 3;

         public readonly AsyncManualResetEvent resetEvent = new AsyncManualResetEvent(false);
         public readonly AsyncAutoResetEvent completingFreedEvent = new AsyncAutoResetEvent();
         public readonly T value;
         public int state = kStatePending;

         public WriterContext(T value) {
            this.value = value;
         } 
      }
   }
}