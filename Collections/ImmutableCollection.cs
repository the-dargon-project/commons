using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections
{
   public class ImmutableCollection
   {
      public static IReadOnlyCollection<T> Of<T>() { return new List<T>(); }
      public static IReadOnlyCollection<T> Of<T>(T t1) { return new List<T> { t1 }; }
      public static IReadOnlyCollection<T> Of<T>(T t1, T t2) { return new List<T> { t1, t2 }; }
      public static IReadOnlyCollection<T> Of<T>(T t1, T t2, T t3) { return new List<T> { t1, t2, t3 }; }
      public static IReadOnlyCollection<T> Of<T>(T t1, T t2, T t3, T t4) { return new List<T> { t1, t2, t3, t4 }; }
      public static IReadOnlyCollection<T> Of<T>(T t1, T t2, T t3, T t4, T t5) { return new List<T> { t1, t2, t3, t4, t5 }; }
   }
}
