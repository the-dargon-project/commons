using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   /**
    * Generated with:
    *
    * var n = 10;
    * var s1 = "public static IReadOnlyDictionary<K, V> Of<K, V>(";
    * var s2 = ") { return new ListDictionary<K, V> { ";
    * var s3 = "}; }\r\n";
    * var result = "";
    * for (var i = 0; i <= n; i++) {
    *   var args = [];
    *   var params = [];
    *   for (var j = 1; j <= i; j++) {
    *     args.push("K k" + j + ", V v" + j);
    *     params.push("[k" + j + "] = v" + j);
    *   }
    *   result += s1 + args.join(", ") + s2 + params.join(", ") + s3;
    * }
    * result
    */
   public static class ImmutableDictionary
   {
      public static IReadOnlyDictionary<K, V> Of<K, V>() { return new ListDictionary<K, V> { }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1) { return new ListDictionary<K, V> {[k1] = v1 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5,[k6] = v6 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5,[k6] = v6,[k7] = v7 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5,[k6] = v6,[k7] = v7,[k8] = v8 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5,[k6] = v6,[k7] = v7,[k8] = v8,[k9] = v9 }; }
      public static IReadOnlyDictionary<K, V> Of<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9, K k10, V v10) { return new ListDictionary<K, V> {[k1] = v1,[k2] = v2,[k3] = v3,[k4] = v4,[k5] = v5,[k6] = v6,[k7] = v7,[k8] = v8,[k9] = v9,[k10] = v10 }; }   }
}