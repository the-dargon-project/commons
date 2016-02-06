using System.Runtime.CompilerServices;

namespace Dargon.Commons
{
   public static partial class DargonCommonsExtensions
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool WithinII(this int value, int lower, int upper)
      {
         return value >= lower && value <= upper;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool WithinIE(this int value, int lower, int upper)
      {
         return value >= lower && value < upper;
      }
   }
}
