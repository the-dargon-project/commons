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
      public static IReadOnlyCollection<T> Of<T>(params T[] args) { return args; }
   }
}
