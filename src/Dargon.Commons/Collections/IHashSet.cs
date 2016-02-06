using System;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IHashSet<T> : ISet<T>, IReadOnlySet<T> {
      int RemoveWhere(Predicate<T> match);
      void TrimExcess();
      IEqualityComparer<T> Comparer { get; }
   }
}