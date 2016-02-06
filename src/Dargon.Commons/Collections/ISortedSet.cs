using System;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface ISortedSet<T> : IReadOnlySet<T>, ISet<T> {
      int RemoveWhere(Predicate<T> match);
      IEnumerable<T> Reverse();
      IComparer<T> Comparer { get; }
      T Max { get; }
      T Min { get; }
   }
}