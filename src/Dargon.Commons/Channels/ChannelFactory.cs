using Dargon.Commons.AsyncPrimitives;
using Dargon.Commons.Collections;
using System;
using System.Threading.Tasks;
using static Dargon.Commons.Channels.ChannelsExtensions;

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

      public static ReadableChannel<bool> Timeout(int millis) => Timeout(TimeSpan.FromMilliseconds(millis));

      public static ReadableChannel<bool> Timeout(TimeSpan interval) {
         var channel = Nonblocking<bool>(1);

         Go(async () => {
            await Task.Delay(interval).ConfigureAwait(false);
//            Console.WriteLine("Time signalling");
            await channel.WriteAsync(true).ConfigureAwait(false);
         });

         return channel;
      }
   }

   public static class Time {
      public static ReadableChannel<bool> After(int millis) => ChannelFactory.Timeout(millis);
   }
}