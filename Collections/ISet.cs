using System.Collections;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlySet<T> {
      new int Count { get; }
      new void CopyTo(T[] array, int arrayIndex);
   }
}
