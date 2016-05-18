using System;
using System.Threading;
using System.Threading.Tasks;
using static Dargon.Commons.Channels.ToFuncTTaskConverter;

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

      public static async Task Run(Func<Task> task) {
         await task().ConfigureAwait(false);
      }

      public static Task Go(Func<Task> task) => Run(task);

      public static ICaseTemporary Case<T>(ReadableChannel<T> channel, Action callback) {
         return new CaseTemporary<T>(channel, Convert<T>(callback));
      }

      public static ICaseTemporary Case<T>(ReadableChannel<T> channel, Action<T> callback) {
         return new CaseTemporary<T>(channel, Convert<T>(callback));
      }

      public static ICaseTemporary Case<T>(ReadableChannel<T> channel, Func<Task> callback) {
         return new CaseTemporary<T>(channel, Convert<T>(callback));
      }

      public static ICaseTemporary Case<T>(ReadableChannel<T> channel, Func<T, Task> callback) {
         return new CaseTemporary<T>(channel, callback);
      }
   }
}