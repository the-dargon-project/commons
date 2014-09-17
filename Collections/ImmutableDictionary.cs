using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections
{
   public static class ImmutableDictionary
   {
      public static IReadOnlyDictionary<K, V> Of<K, V>() { return new ListDictionary<K, V>(); }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1) { return new ListDictionary<K, V>().With(d => d.Add(k1, v1)); }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2) { return new ListDictionary<K, V>().With(d => d.Add(k1, v1)).With(d => d.Add(k2, v2)); }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) { return new ListDictionary<K, V>().With(d => d.Add(k1, v1)).With(d => d.Add(k2, v2)).With(d => d.Add(k3, v3)); }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) { return new ListDictionary<K, V>().With(d => d.Add(k1, v1)).With(d => d.Add(k2, v2)).With(d => d.Add(k3, v3)).With(d => d.Add(k4, v4)); }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) { return new ListDictionary<K, V>().With(d => d.Add(k1, v1)).With(d => d.Add(k2, v2)).With(d => d.Add(k3, v3)).With(d => d.Add(k4, v4)).With(d => d.Add(k5, v5)); }
   }
}
