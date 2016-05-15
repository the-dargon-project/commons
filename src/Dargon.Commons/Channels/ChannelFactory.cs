using System.Collections.Concurrent;
using Nito.AsyncEx;

namespace Dargon.Commons.Channels {
   public static class ChannelFactory {
      public static Channel<T> Nonblocking<T>() => Nonblocking<T>(-1);

      public static Channel<T> Nonblocking<T>(int maxEnqueuedSize) {
         var queue = new ConcurrentQueue<T>();
         var readSemaphore = new AsyncSemaphore(0);
         var writeSemaphore = maxEnqueuedSize > 0 ? new AsyncSemaphore(maxEnqueuedSize) : null;
         return new NonblockingChannel<T>(queue, readSemaphore, writeSemaphore);
      }

      public static Channel<T> Blocking<T>() {
         return new BlockingChannel<T>();
      } 
   }
}