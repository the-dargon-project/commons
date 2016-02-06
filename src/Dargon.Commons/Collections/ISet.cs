using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlySet<T> {
      new int Count { get; }
      new void CopyTo(T[] array, int arrayIndex);
      new bool Contains(T element);
   }
}
