using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IConcurrentSet<T> : ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable, ISet<T> {
      bool IsEmpty { get; }
      bool TryAdd(T item);
      bool TryRemove(T item);
   }
}