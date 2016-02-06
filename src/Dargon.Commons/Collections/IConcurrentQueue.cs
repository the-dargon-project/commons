using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IConcurrentQueue<T> : IEnumerable<T> {
      void CopyTo(T[] array, int index);
      void Enqueue(T item);
      T[] ToArray();
      bool TryDequeue(out T result);
      bool TryPeek(out T result);
      int Count { get; }
      bool IsEmpty { get; }
   }
}