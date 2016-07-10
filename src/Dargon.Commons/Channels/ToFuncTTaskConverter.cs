using System;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public static class ToFuncTTaskConverter {
      public static Func<T, Task> Convert<T>(Action callback) {
         return Convert<T>(t => callback());
      }

      public static Func<T, Task> Convert<T>(Action<T> callback) {
         return t => {
            callback(t);
            return Task.CompletedTask;
         };
      }

      public static Func<T, Task> Convert<T>(Func<Task> callback) {
         return t => callback();
      }
   }
}
