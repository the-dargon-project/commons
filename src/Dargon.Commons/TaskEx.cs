using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons {
   /// <summary>
   /// Via http://codereview.stackexchange.com/questions/110066/async-yield-and-continue-on-taskpool
   /// </summary>
   public static class TaskEx {
      public static Task<bool> False = Task.FromResult(false);
      public static Task<bool> True = Task.FromResult(true);

      /// <summary>
      /// Yields and schedules the continuation on the <see cref="ThreadPool"/>.
      /// </summary>
      /// <returns><see cref="ToThreadPoolAwaitable"/>.</returns>
      public static ToThreadPoolAwaitable YieldToThreadPool() {
         return new ToThreadPoolAwaitable();
      }

      public struct ToThreadPoolAwaitable {
         public ToThreadPoolAwaiter GetAwaiter() {
            return new ToThreadPoolAwaiter();
         }

         [HostProtection(Synchronization = true, ExternalThreading = true)]
         public struct ToThreadPoolAwaiter : ICriticalNotifyCompletion {
            /// <summary>Gets whether a yield is not required.</summary>
            /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
            public bool IsCompleted => false; // always yield
            //public bool IsCompleted => return Thread.CurrentThread.IsThreadPoolThread; // Alternatively, only yield if not on ThreadPool.    

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecuritySafeCritical]
            public void OnCompleted(Action continuation) {
               QueueContinuation(continuation, flowContext: true);
            }

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecurityCritical]
            public void UnsafeOnCompleted(Action continuation) {
               QueueContinuation(continuation, flowContext: false);
            }

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <param name="flowContext">true to flow ExecutionContext; false if flowing is not required.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecurityCritical]
            private static void QueueContinuation(Action continuation, bool flowContext) {
               // Validate arguments
               if (continuation == null) throw new ArgumentNullException(nameof(continuation));

               if (flowContext) {
                  ThreadPool.QueueUserWorkItem(WaitCallbackRunAction, continuation);
               } else {
                  ThreadPool.UnsafeQueueUserWorkItem(WaitCallbackRunAction, continuation);
               }
            }

            /// <summary>WaitCallback that invokes the Action supplied as object state.</summary>
            private static readonly WaitCallback WaitCallbackRunAction = RunAction;

            /// <summary>Runs an Action delegate provided as state.</summary>
            /// <param name="state">The Action delegate to invoke.</param>
            private static void RunAction(object state) {
               ((Action)state)();
            }

            /// <summary>Ends the await operation.</summary>
            public void GetResult() { } // Nop. It exists purely because the compiler pattern demands it.
         }
      }
   }
}
