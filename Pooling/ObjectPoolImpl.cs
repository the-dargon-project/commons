using System;
using ItzWarty.Collections;

namespace ItzWarty.Pooling {
   public class ObjectPoolImpl<T> : ObjectPool<T> {
      private readonly Func<T> generator;
      private readonly IConcurrentBag<T> container;
      private readonly string name;

      public ObjectPoolImpl(Func<T> generator) : this(generator, new ConcurrentBag<T>(), null) {}
      public ObjectPoolImpl(Func<T> generator, IConcurrentBag<T> container) : this(generator, container, null) { }
      public ObjectPoolImpl(Func<T> generator, string name) : this(generator, new ConcurrentBag<T>(), name) { }
      public ObjectPoolImpl(Func<T> generator, IConcurrentBag<T> container, string name) {
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
            result = generator();
         }
         return result;
      }

      public void ReturnObject(T item) {
         container.Add(item);
      }
   }
}