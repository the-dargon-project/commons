using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Drawing;
using System.IO;

namespace ItzWarty
{
   public static class Extend
   {
      // http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
      public static string ToHex(this byte[] a)
      {
         var hex = new StringBuilder(a.Length * 2);
         foreach (byte b in a)
            hex.AppendFormat("{0:x2}", b);
         return hex.ToString();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ulong GetUnixTime(this DateTime dateTime)
      {
         return (ulong)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ulong GetUnixTimeMilliseconds(this DateTime dateTime)
      {
         return (ulong)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
      }

      //http://stackoverflow.com/questions/128618/c-file-size-format-provider
      public static string ToFileSize(this long l)
      {
         return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
      }

      private delegate K TryGetValueDelegate<K, V>(K key, out V value);


      public static bool Within(this double a, double b, double epsilon)
      {
         return Math.Abs(a - b) <= epsilon;
      }

      public static bool Within(this float a, float b, float epsilon)
      {
         return Math.Abs(a - b) <= epsilon;
      }
   }
}
