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

      public static Task Run(Func<Task> task) {
         return Task.Run(task);
//         TaskCompletionSource<byte> tcs = new TaskCompletionSource<byte>(TaskCreationOptions.RunContinuationsAsynchronously);
//         Task.Run(async () => {
//            await task().ConfigureAwait(false);
//            tcs.SetResult(0);
//         });
//         return tcs.Task;
      }

      public static Task<T> Run<T>(Func<Task<T>> task) {
         return Task.Run(task);
         //         TaskCompletionSource<T> tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
         //         Task.Run(async () => {
         //            tcs.SetResult(await task().ConfigureAwait(false));
         //         });
         //         return tcs.Task;
      }

      public static Task Go(Func<Task> task) => Run(task);

      public static Task<T> Go<T>(Func<Task<T>> task) => Run(task);

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