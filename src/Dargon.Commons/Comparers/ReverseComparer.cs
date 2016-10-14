using System;
using System.Collections.Generic;

namespace Dargon.Commons.Comparers {
   public class ReverseComparer<T> : IComparer<T> {
      private readonly IComparer<T> m_originalComparer;

      public ReverseComparer(IComparer<T> originalComparer) {
         m_originalComparer = originalComparer ?? Comparer<T>.Default;
      }

      public int Compare(T x, T y) {
         return m_originalComparer.Compare(y, x);
      }
   }
}
