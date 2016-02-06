using System;
using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, IReadOnlyDictionary<TKey, TValue>, ICollection, IEnumerable {
      TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
      TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);
      new void Clear();
      new bool ContainsKey(TKey key);
      new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
      TValue GetOrAdd(TKey key, TValue value);
      TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
      KeyValuePair<TKey, TValue>[] ToArray();
      bool TryAdd(TKey key, TValue value);
      new bool TryGetValue(TKey key, out TValue value);
      bool TryRemove(TKey key, out TValue value);
      bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
      new int Count { get; }
      bool IsEmpty { get; }
      new bool IsReadOnly { get; }
      new ICollection<TKey> Keys { get; }
      new ICollection<TValue> Values { get; }
      new TValue this[TKey key] { get; set; }
   }
}