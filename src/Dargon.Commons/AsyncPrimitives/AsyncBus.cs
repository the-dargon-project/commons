using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dargon.Commons.AsyncPrimitives {
   public class AsyncBus<T> : IAsyncBus<T> {
      public delegate Task Subscription(IAsyncSubscriber<T> self, T thing);

      private readonly LinkedList<Subscription> subscriptions = new LinkedList<Subscription>(); 

      public Task PostAsync(T thing) {
         var tasks = subscriptions.Select(s => DargonCommonsExtensions.Forgettable(s(this, thing)));
         return Task.WhenAll(tasks);
      }

      public void Subscribe(Func<IAsyncSubscriber<T>, T, Task> handler) {
         subscriptions.AddFirst(new Subscription(handler));
      }
   }
}