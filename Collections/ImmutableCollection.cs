using System.Collections.Generic;

namespace ItzWarty.Collections
{
   public class ImmutableCollection
   {
      public static IReadOnlyCollection<T> Of<T>() { return new List<T>(); }
      public static IReadOnlyCollection<T> Of<T>(params T[] args) { return args; }
   }
}
