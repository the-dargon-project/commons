using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IMultiValueDictionary<TKey, TValue> {
      /// <summary>
      /// Adds the specified value under the specified key
      /// </summary>
      /// <param name="key">The key.</param>
      /// <param name="value">The value.</param>
      void Add(TKey key, TValue value);

      /// <summary>
      /// Determines whether this dictionary contains the specified value for the specified key 
      /// </summary>
      /// <param name="key">The key.</param>
      /// <param name="value">The value.</param>
      /// <returns>true if the value is stored for the specified key in this dictionary, false otherwise</returns>
      bool ContainsValue(TKey key, TValue value);

      /// <summary>
      /// Removes the specified value for the specified key. It will leave the key in the dictionary.
      /// </summary>
      /// <param name="key">The key.</param>
      /// <param name="value">The value.</param>
      void Remove(TKey key, TValue value);

      /// <summary>
      /// Gets the values for the key specified. This method is useful if you want to avoid an exception for key value retrieval and you can't use TryGetValue
      /// (e.g. in lambdas)
      /// </summary>
      /// <param name="key">The key.</param>
      /// <param name="returnEmptySet">if set to true and the key isn't found, an empty hashset is returned, otherwise, if the key isn't found, null is returned</param>
      /// <returns>
      /// This method will return null (or an empty set if returnEmptySet is true) if the key wasn't found, or
      /// the values if key was found.
      /// </returns>
      HashSet<TValue> GetValues(TKey key, bool returnEmptySet);

      void Clear();
      bool ContainsKey(TKey key);
      Dictionary<TKey, HashSet<TValue>>.Enumerator GetEnumerator();
      bool Remove(TKey key);
      bool TryGetValue(TKey key, out HashSet<TValue> value);
      IEqualityComparer<TKey> Comparer { get; }
      int Count { get; }
      Dictionary<TKey, HashSet<TValue>>.KeyCollection Keys { get; }
      Dictionary<TKey, HashSet<TValue>>.ValueCollection Values { get; }
      HashSet<TValue> this[TKey key] { get; set; }
   }
}