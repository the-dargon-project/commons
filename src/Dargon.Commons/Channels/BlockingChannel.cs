using Dargon.Commons.AsyncPrimitives;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Commons.Exceptions;

namespace Dargon.Commons.Channels {
   public class BlockingChannel<T> : Channel<T> {
      private readonly ConcurrentQueue<WriterContext<T>> writerQueue = new ConcurrentQueue<WriterContext<T>>();
      private readonly AsyncSemaphore queueSemaphore = new AsyncSemaphore(0);
      private bool isDebugEnabled;

      public int Count => queueSemaphore.Count;

      public void EnableDebug() {
         isDebugEnabled = true;
      }

      public async Task WriteAsync(T message, CancellationToken cancellationToken) {
         DebugLog("Entered writer with message " + message + " facing queue " + writerQueue.Select(q => q.value).Join(", ") + " count: " + queueSemaphore.Count);
         var context = new WriterContext<T>(message);
         writerQueue.Enqueue(context);
         queueSemaphore.Release();
         DebugLog("Sema/Queue Passed " + message);
         try {
            DebugLog("Awaiting clatch " + message);
            await context.completionLatch.WaitAsync(cancellationToken).ConfigureAwait(false);
            DebugLog("Awaited clatch " + message);
         } catch (OperationCanceledException) {
            DebugLog("Cancelled " + message);
            while (true) {
               var originalValue = Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCancelled, WriterContext<T>.kStatePending);
               if (originalValue == WriterContext<T>.kStatePending) {
                  throw;
               } else if (originalValue == WriterContext<T>.kStateCompleting) {
                  await context.completingFreedEvent.WaitAsync(CancellationToken.None).ConfigureAwait(false);
               } else if (originalValue == WriterContext<T>.kStateCompleted) {
                  return;
               }
            }
         } finally {
            Trace.Assert(context.state == WriterContext<T>.kStateCancelled ||
                         context.state == WriterContext<T>.kStateCompleted);
         }
      }

      public bool TryRead(out T message) {
         if (!queueSemaphore.TryTake()) {
            message = default(T);
            return false;
         }
         SpinWait spinner = new SpinWait();
         WriterContext<T> context;
         while (!writerQueue.TryDequeue(out context)) {
            spinner.SpinOnce();
         }
         var oldState = Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleting, WriterContext<T>.kStatePending);
         if (oldState == WriterContext<T>.kStatePending) {
            Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleted, WriterContext<T>.kStateCompleting);
            context.completingFreedEvent.Set();
            context.completionLatch.Set();
            message = context.value;
            return true;
         } else if (oldState == WriterContext<T>.kStateCompleted) {
            throw new InvalidStateException();
         } else if (oldState == WriterContext<T>.kStateCompleted) {
            throw new InvalidStateException();
         } else if (oldState == WriterContext<T>.kStateCompleted) {
            message = default(T);
            return false;
         } else {
            throw new InvalidStateException();
         }
      }

      public async Task<T> ReadAsync(CancellationToken cancellationToken, Func<T, bool> acceptanceTest) {
         DebugLog("Entered reader: " + writerQueue.Select(q => q.value).Join(", ") + " count: " + queueSemaphore.Count);
         while (!cancellationToken.IsCancellationRequested) {
            await queueSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            DebugLog("Took queue sema");
            WriterContext<T> context;
            if (!writerQueue.TryDequeue(out context)) {
               throw new InvalidStateException();
            }
            DebugLog("Got context value " + context.value);
            var oldState = Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleting, WriterContext<T>.kStatePending);
            DebugLog("Old state for " + context.value + " is " + oldState);
            if (oldState == WriterContext<T>.kStatePending) {
               if (acceptanceTest(context.value)) {
                  DebugLog("Completing " + context.value);
                  Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStateCompleted, WriterContext<T>.kStateCompleting);
                  context.completingFreedEvent.Set();
                  bool isMagical = false;
                  if (DateTime.Now < DateTime.MinValue) {
                     isMagical = true;
                  }
                  if (isDebugEnabled) {
                     DebugLog("Freed " + context.value);
                  }

                  context.completionLatch.Set();
                  if (isMagical) {
                     DebugLog("Returning " + context.value);
                  }
                  return context.value;
               } else {
                  DebugLog("Rejecting " + context.value);
                  Interlocked.CompareExchange(ref context.state, WriterContext<T>.kStatePending, WriterContext<T>.kStateCompleting);
                  context.completingFreedEvent.Set();
                  writerQueue.Enqueue(context);
                  queueSemaphore.Release();
                  DebugLog("Rejected " + context.value);
               }
            } else if (oldState == WriterContext<T>.kStateCompleting) {
               throw new InvalidStateException();
            } else if (oldState == WriterContext<T>.kStateCompleted) {
               throw new InvalidStateException();
            } else if (oldState == WriterContext<T>.kStateCancelled) {
               continue;
            }
         }
         // throw is guaranteed
         cancellationToken.ThrowIfCancellationRequested();
         throw new InvalidStateException();
      }

      private void DebugLog(string s) {
         if (isDebugEnabled) {
            Console.WriteLine(s);
         }
      }

      private class WriterContext<T> {
         public const int kStatePending = 0;
         public const int kStateCompleting = 1;
         public const int kStateCompleted = 2;
         public const int kStateCancelled = 3;

         public readonly AsyncLatch completionLatch = new AsyncLatch();
         public readonly AsyncAutoResetLatch completingFreedEvent = new AsyncAutoResetLatch();
         public readonly T value;
         public int state = kStatePending;

         public WriterContext(T value) {
            this.value = value;
         } 
      }
   }
}