using System;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public class CaseTemporary<T> : ICaseTemporary {
      private readonly ReadableChannel<T> channel;
      private readonly Func<T, Task> callback;

      public CaseTemporary(ReadableChannel<T> channel, Func<T, Task> callback) {
         this.channel = channel;
         this.callback = callback;
      }

      public void Register(DispatchContext dispatchContext) {
         dispatchContext.Case(channel, callback);
      }
   }
}