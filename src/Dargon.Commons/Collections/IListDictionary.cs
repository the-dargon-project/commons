using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IListDictionary<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V> {
      V Get(K key);
      void AddOrUpdate(K key, V value);
   }
}