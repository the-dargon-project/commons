using System;
using System.Collections.Generic;
using System.Threading;
using Dargon.Commons.Collections;

namespace Dargon.Commons.Pooling {
   public class StackBackedObjectPool<T> : IObjectPool<T> {
      private readonly ConcurrentQueue<T> container = new ConcurrentQueue<T>();
      private readonly Func<IObjectPool<T>, T> generator;
      private readonly string name;

      public StackBackedObjectPool(Func<IObjectPool<T>, T> generator) : this(generator, null) { }
      public StackBackedObjectPool(Func<IObjectPool<T>, T> generator, string name) {
         generator.ThrowIfNull("generator");

         this.generator = generator;
         this.name = name;
      }

      public string Name => name;
      public int Count => container.Count;

      public T TakeObject() {
         T r;
         if (!container.TryDequeue(out r)) {
            r = generator(this);
         }
         return r;
      }
      
      public void ReturnObject(T item) {
         container.Enqueue(item);
      }
   }
}