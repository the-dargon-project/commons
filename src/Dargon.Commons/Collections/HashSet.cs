using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class HashSet<T> : System.Collections.Generic.HashSet<T>, IHashSet<T> {
      public HashSet() { }
      public HashSet(IEnumerable<T> collection) : base(collection) { }
      public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : base(collection, comparer) { }
      public HashSet(IEqualityComparer<T> comparer) : base(comparer) { }
   }
}
