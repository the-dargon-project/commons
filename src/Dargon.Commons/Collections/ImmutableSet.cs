namespace Dargon.Commons.Collections {
   public class ImmutableSet {
      public static IReadOnlySet<T> Of<T>() {
         return new HashSet<T>();
      }

      public static IReadOnlySet<T> Of<T>(params T[] values) {
         return new HashSet<T>(values);
      }
   }
}