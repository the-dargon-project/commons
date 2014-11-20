using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections {
   public class SortedDictionary<K, V> : System.Collections.Generic.SortedDictionary<K, V>, IReadOnlyDictionary<K, V> {
      public SortedDictionary() {}
      public SortedDictionary(IComparer<K> comparer) : base(comparer) {}
      public SortedDictionary(IDictionary<K, V> dictionary) : base(dictionary) { }
      public SortedDictionary(IDictionary<K, V> dictionary, IComparer<K> comparer) : base(dictionary, comparer) {}

      IEnumerable<K> IReadOnlyDictionary<K, V>.Keys { get { return this.Keys; } }
      IEnumerable<V> IReadOnlyDictionary<K, V>.Values { get { return this.Values; } }
   }
}
