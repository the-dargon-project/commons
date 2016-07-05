using System;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public interface IAsyncPoster<T> {
      Task PostAsync(T thing);
   }

   public interface IAsyncSubscriber<T> {
      void Subscribe(Func<IAsyncSubscriber<T>, T, Task> handler);
   }

   public interface IAsyncBus<T> : IAsyncPoster<T>, IAsyncSubscriber<T> { }

   public static class EventBusStatics {
      public static IAsyncPoster<T> Poster<T>(this IAsyncBus<T> bus) => bus;
      public static IAsyncSubscriber<T> Subscriber<T>(this IAsyncBus<T> bus) => bus;
   }
}
