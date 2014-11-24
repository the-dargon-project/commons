using System;
using System.Collections;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public interface IHashSet<T> : ISet<T>, IReadOnlySet<T> {
      int RemoveWhere(Predicate<T> match);
      void TrimExcess();
      IEqualityComparer<T> Comparer { get; }
   }
}