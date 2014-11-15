using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using ItzWarty.FormatProviders;

namespace ItzWarty
{
   public static class Extend {
      public static T[] Cast<T, U>(this U[] values, Func<U, T> cast) {
         T[] result = new T[values.Length];
         for (int i = 0; i < result.Length; i++)
            result[i] = cast(values[i]);
         return result;
      }
      public static KeyValuePair<TKey, TValue> PairValue<TKey, TValue>(this TKey key, TValue value) { return new KeyValuePair<TKey, TValue>(key, value); }
      public static KeyValuePair<TKey, TValue> PairKey<TKey, TValue>(this TValue value, TKey key) { return key.PairValue(value); }

      /// <summary>
      /// Calls the given function, passing self as the argument.
      /// </summary>
      public static T With<T>(this T self, Action<T> func) {
         func(self);
         return self;
      }

      /// <summary>
      /// Calls the given function, passing self as the argument.
      /// </summary>
      public static U With<T, U>(this T self, Func<T, U> func) {
         return func(self);
      }

      /// <summary>
      /// Runs self through the function, and returns the result.
      /// </summary>
      /// <typeparam name="T">The type of the fileName parameter</typeparam>
      /// <typeparam name="U">The type of the output result</typeparam>
      /// <param name="self">The fileName parameter which is passed through func</param>
      /// <param name="func">The function which we pass our fileName parameter through.</param>
      /// <returns>func(self)</returns>
      public static U Pass<T, U>(this T self, Func<T, U> func) {
         return func(self);
      }

      /// <summary>                                                                                              
      /// Checks whether argument is <see langword="null"/> and throws <see cref="ArgumentNullException"/> if so.
      /// </summary>                                                                                             
      /// <param name="argument">Argument to check on <see langword="null"/>.</param>                            
      /// <param name="argumentName">Argument name to pass to Exception constructor.</param>                     
      /// <returns>Specified argument.</returns>                                                                 
      /// <exception cref="ArgumentNullException"/>                                                              
      [DebuggerStepThrough]
      public static T ThrowIfNull<T>(this T argument, string argumentName)
         where T : class {
         if (argument == null) {
            throw new ArgumentNullException(argumentName);
         } else {
            return argument;
         }
      }


      public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                 Func<TSource, TKey> selector) {
         return source.MinBy(selector, Comparer<TKey>.Default);
      }

      public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                 Func<TSource, TKey> selector, IComparer<TKey> comparer) {
         source.ThrowIfNull("source");
         selector.ThrowIfNull("selector");
         comparer.ThrowIfNull("comparer");
         using (IEnumerator<TSource> sourceIterator = source.GetEnumerator()) {
            if (!sourceIterator.MoveNext()) {
               throw new InvalidOperationException("Sequence was empty");
            }
            TSource min = sourceIterator.Current;
            TKey minKey = selector(min);
            while (sourceIterator.MoveNext()) {
               TSource candidate = sourceIterator.Current;
               TKey candidateProjected = selector(candidate);
               if (comparer.Compare(candidateProjected, minKey) < 0) {
                  min = candidate;
                  minKey = candidateProjected;
               }
            }
            return min;
         }
      }

      public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                 Func<TSource, TKey> selector) {
         return source.MaxBy(selector, Comparer<TKey>.Default);
      }

      public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                 Func<TSource, TKey> selector, IComparer<TKey> comparer) {
         source.ThrowIfNull("source");
         selector.ThrowIfNull("selector");
         comparer.ThrowIfNull("comparer");
         using (IEnumerator<TSource> sourceIterator = source.GetEnumerator()) {
            if (!sourceIterator.MoveNext()) {
               throw new InvalidOperationException("Sequence was empty");
            }
            TSource max = sourceIterator.Current;
            TKey maxKey = selector(max);
            while (sourceIterator.MoveNext()) {
               TSource candidate = sourceIterator.Current;
               TKey candidateProjected = selector(candidate);
               if (comparer.Compare(candidateProjected, maxKey) > 0) {
                  max = candidate;
                  maxKey = candidateProjected;
               }
            }
            return max;
         }
      }

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
