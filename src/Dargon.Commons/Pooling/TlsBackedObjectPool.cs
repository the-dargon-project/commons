using System;
using System.Collections.Generic;
using System.Threading;

namespace Dargon.Commons.Pooling {
   public class TlsBackedObjectPool<T> : IObjectPool<T> {
      private readonly ThreadLocal<Stack<T>> container = new ThreadLocal<Stack<T>>(() => new Stack<T>(), false);
      private readonly Func<IObjectPool<T>, T> generator;
      private readonly string name;

      public TlsBackedObjectPool(Func<IObjectPool<T>, T> generator) : this(generator, null) {}
      public TlsBackedObjectPool(Func<IObjectPool<T>, T> generator, string name) {
         generator.ThrowIfNull("generator");
         
         this.generator = generator;
         this.name = name;
      }

      public string Name => name;
      public int Count => container.Value.Count;

      public T TakeObject() {
         var s = container.Value;
         if (s.None()) {
            return generator(this);
         }
         return s.Pop();
      }

      public void ReturnObject(T item) {
         container.Value.Push(item);
      }
   }
}