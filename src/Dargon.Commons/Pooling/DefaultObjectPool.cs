using System;
using Dargon.Commons.Collections;

namespace Dargon.Commons.Pooling {
   public class DefaultObjectPool<T> : IObjectPool<T> {
      private readonly Func<IObjectPool<T>, T> generator;
      private readonly IConcurrentBag<T> container;
      private readonly string name;

      public DefaultObjectPool(Func<IObjectPool<T>, T> generator) : this(generator, new ConcurrentBag<T>(), null) {}
      public DefaultObjectPool(Func<IObjectPool<T>, T> generator, IConcurrentBag<T> container) : this(generator, container, null) { }
      public DefaultObjectPool(Func<IObjectPool<T>, T> generator, string name) : this(generator, new ConcurrentBag<T>(), name) { }
      public DefaultObjectPool(Func<IObjectPool<T>, T> generator, IConcurrentBag<T> container, string name) {
         generator.ThrowIfNull("generator");
         container.ThrowIfNull("container");

         this.generator = generator;
         this.container = container;
         this.name = name;
      }

      public string Name => name;
      public int Count => container.Count;

      public T TakeObject() {
         T result;
         if (!container.TryTake(out result)) {
            result = generator(this);
         }
         return result;
      }

      public void ReturnObject(T item) {
         container.Add(item);
      }
   }
}