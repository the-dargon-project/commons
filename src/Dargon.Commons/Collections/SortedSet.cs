using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class SortedSet<T> : System.Collections.Generic.SortedSet<T>, ISortedSet<T> {
      public SortedSet() { } 
      public SortedSet(IComparer<T> comparer) : base(comparer) { } 
      public SortedSet(IEnumerable<T> collection) : base(collection) { }
      public SortedSet(IEnumerable<T> collection, IComparer<T> comparer) : base(collection, comparer) { }
   }
}
