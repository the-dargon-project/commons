using System.Collections.Generic;

namespace Dargon.Commons.Collections {
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

      public bool IsReadOnly => false;

      bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) { return ContainsKey(key); }
      bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) { return TryGetValue(key, out value); }

      IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
      IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
      int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => Count;
      TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => this[key];
   }
}
