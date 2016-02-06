using System;
using System.Collections.Generic;

namespace Dargon.Commons.Comparers {
   public class LambdaEqualityComparer<T> : IEqualityComparer<T> {
      private readonly Func<T, int> hashcode;
      private readonly Func<T, T, bool> equals;

      public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> hashcode) {
         this.hashcode = hashcode;
         this.equals = equals;
      }

      public bool Equals(T x, T y) {
         return equals(x, y);
      }

      public int GetHashCode(T obj) {
         return hashcode(obj);
      }
   }
}