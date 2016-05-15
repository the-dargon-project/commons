using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public static class ChannelsExtensions {
      public static void Write<T>(this WritableChannel<T> channel, T message) {
         channel.WriteAsync(message).Wait();
      }

      public static Task WriteAsync<T>(this WritableChannel<T> channel, T message) {
         return channel.WriteAsync(message, CancellationToken.None);
      }

      public static T Read<T>(this ReadableChannel<T> channel) {
         return channel.ReadAsync().Result;
      }

      public static Task<T> ReadAsync<T>(this ReadableChannel<T> channel) {
         return channel.ReadAsync(CancellationToken.None, acceptanceTest => true);
      }

      public static Task Run(Func<Task> task) {
         return task();
      }

      public static Task Go(Func<Task> task) => Run(task);
   }
}