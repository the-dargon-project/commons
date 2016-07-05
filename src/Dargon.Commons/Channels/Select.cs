using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public class Select : INotifyCompletion, IEnumerable {
      private readonly DispatchContext dispatchContext;

      public Select(int n = 1) {
         dispatchContext = Dispatch.Times(n);
      }

      #region List Initializer Support
      public void Add(ICaseTemporary c) {
         c.Register(dispatchContext);
      }

      // IEnumerable interface is required for collection initializer.
      public IEnumerator GetEnumerator() {
         throw new NotImplementedException("Attempted to enumerate on Channels Select");
      }
      #endregion

      //-------------------------------------------------------------------------------------------
      // Await option 1 - conversion to Task
      //-------------------------------------------------------------------------------------------
      public static implicit operator Task(Select select) {
         return select.WaitAsync();
      }

      //-------------------------------------------------------------------------------------------
      // Await option 2 - allow await on Select object.
      // According to Stephen Cleary compilers emit this on await:
      //
      //   var temp = e.GetAwaiter();
      //   if (!temp.IsCompleted)
      //   {
      //     SAVE_STATE()
      //     temp.OnCompleted(&cont);
      //     return;
      //   
      //   cont:
      //     RESTORE_STATE()
      //   }
      //   var i = temp.GetResult();
      //
      // http://stackoverflow.com/questions/12661348/custom-awaitables-for-dummies
      //-------------------------------------------------------------------------------------------
      public Select GetAwaiter() { return this; }

      public void GetResult() { }

      public bool IsCompleted => dispatchContext.IsCompleted;

      public void OnCompleted(Action continuation) {
         dispatchContext.WaitAsync().ContinueWith(task => continuation());
      }
      
      public Task WaitAsync(CancellationToken cancellationToken = default(CancellationToken)) {
         return dispatchContext.WaitAsync(cancellationToken);
      }

      //-------------------------------------------------------------------------------------------
      // Syntax Option 3: Select.Case<T>(channel, callback).Case(channel, callback)...
      //-------------------------------------------------------------------------------------------
      public static DispatchContext Case<T>(ReadableChannel<T> channel, Action<T> callback) {
         return Dispatch.Once().Case(channel, callback);
      }

      public static DispatchContext Case<T>(ReadableChannel<T> channel, Func<Task> callback) {
         return Dispatch.Once().Case(channel, callback);
      }

      public static DispatchContext Case<T>(ReadableChannel<T> channel, Func<T, Task> callback) {
         return Dispatch.Once().Case(channel, callback);
      }
   }

   public static class Dispatch {
      public static DispatchContext Once() => Times(1);

      public static DispatchContext Forever() => Times(DispatchContext.kTimesInfinite);

      public static DispatchContext Times(int n) {
         return new DispatchContext(n);
      }
   }
}