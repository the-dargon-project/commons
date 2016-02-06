using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IReadOnlySet<T> : IReadOnlyCollection<T> {
      /*
      bool IsProperSubsetOf(IEnumerable<T> other);
      bool IsProperSupersetOf(IEnumerable<T> other);
      bool IsSubsetOf(IEnumerable<T> other);
      bool IsSupersetOf(IEnumerable<T> other);
      bool Overlaps(IEnumerable<T> other);
       */
      bool SetEquals(IEnumerable<T> other);
      void CopyTo(T[] array);
      void CopyTo(T[] array, int arrayIndex);
      void CopyTo(T[] array, int arrayIndex, int count);
      bool Contains(T element);
   }
}