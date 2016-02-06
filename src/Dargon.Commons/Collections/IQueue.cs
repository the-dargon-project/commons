using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IQueue<T> : IEnumerable<T>, IEnumerable {
      void Clear();
      bool Contains(T item);
      void CopyTo(T[] array, int arrayIndex);
      T Dequeue();
      void Enqueue(T item);
      T Peek();
      T[] ToArray();
      void TrimExcess();
      int Count { get; }
   }
}