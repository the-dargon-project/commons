using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IOrderedMultiValueDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ISet<TValue>>> {
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

      IReadOnlyCollection<TKey> Keys { get; }
      ICollection<ISet<TValue>> Values { get; }
      ISet<TValue> this[TKey key] { get; }
      new IEnumerator<KeyValuePair<TKey, ISet<TValue>>> GetEnumerator();
   }
}