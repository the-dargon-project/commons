using System;
using System.Collections;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public interface IConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable {
      TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
      TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);
      void Clear();
      bool ContainsKey(TKey key);
      IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
      TValue GetOrAdd(TKey key, TValue value);
      TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
      KeyValuePair<TKey, TValue>[] ToArray();
      bool TryAdd(TKey key, TValue value);
      bool TryGetValue(TKey key, out TValue value);
      bool TryRemove(TKey key, out TValue value);
      bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
      int Count { get; }
      bool IsEmpty { get; }
      ICollection<TKey> Keys { get; }
      ICollection<TValue> Values { get; }
      TValue this[TKey key] { get; set; }
   }
}