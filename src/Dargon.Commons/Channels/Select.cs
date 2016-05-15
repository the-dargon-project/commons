using System;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public static class Select {
      public static DispatchContext Case<T>(ReadableChannel<T> channel, Action<T> callback) {
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