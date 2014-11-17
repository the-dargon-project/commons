using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections {
   public class ConcurrentDictionary<TKey, TValue> : System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>, IConcurrentDictionary<TKey, TValue> {
      public ConcurrentDictionary() 
         : base() { }

      public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) 
         : base(collection) { }

      public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) 
         : base(collection, comparer) { }

      public ConcurrentDictionary(IEqualityComparer<TKey> comparer) 
         : base(comparer) { }

      public ConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) 
         : base(concurrencyLevel, collection, comparer) { }

      public ConcurrentDictionary(int concurrencyLevel, int capacity) 
         : base(concurrencyLevel, capacity) { }

      public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer) 
         : base(concurrencyLevel, capacity, comparer) { }
   }
}
