using System.Collections;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public interface IConcurrentSet<T> : ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable {
      bool IsEmpty { get; }
      bool TryAdd(T item);
      bool TryRemove(T item);
   }
}