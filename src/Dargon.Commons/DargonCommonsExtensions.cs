using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dargon.Commons.FormatProviders;

namespace Dargon.Commons {
   public static partial class DargonCommonsExtensions {
      public static T[] Cast<T, U>(this U[] values, Func<U, T> cast) {
         T[] result = new T[values.Length];
         for (int i = 0; i < result.Length; i++)
            result[i] = cast(values[i]);
         return result;
      }

      public static KeyValuePair<TKey, TValue> PairValue<TKey, TValue>(this TKey key, TValue value) {
         return new KeyValuePair<TKey, TValue>(key, value);
      }

      public static KeyValuePair<TKey, TValue> PairKey<TKey, TValue>(this TValue value, TKey key) {
         return key.PairValue(value);
      }
      
      public static T With<T>(this T self, Action<T> func) {
         func(self);
         return self;
      }

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

      public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector) {
         return source.MinBy(selector, Comparer<TKey>.Default);
      }

      /// <summary>
      /// From morelinq 
      /// http://stackoverflow.com/questions/914109/how-to-use-linq-to-select-object-with-minimum-or-maximum-property-value
      /// </summary>
      public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer) {
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

      public static object VisitType(this object visitedThing, object target, string targetMethodName, params object[] args) {
         return VisitDispatchHelper(new [] { visitedThing.GetType() }, target, targetMethodName, args);
      }

      public static object VisitGeneric(this object visitedThing, Type visitedInterfaceGenericDefinition, object target, string targetMethodName, params object[] args) {
         var visitedType = visitedThing as Type ?? visitedThing.GetType();
         var interfaces = visitedType.GetTypeInfo().GetInterfaces();
         var interfaceMatch = interfaces.First(x => x.GetGenericTypeDefinition() == visitedInterfaceGenericDefinition);
         return VisitDispatchHelper(interfaceMatch.GetGenericArguments(), target, targetMethodName, args);
      }

      private static object VisitDispatchHelper(Type[] genericArguments, object target, string targetMethodName, object[] args) {
         Type targetType = (target as Type) ?? target.GetType();
         target = target is Type ? null : target;
         if (targetType.IsGenericTypeDefinition) {
            targetType = targetType.MakeGenericType(genericArguments);
            var targetMethods = targetType.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var targetMethod = targetMethods.First(x => x.Name == targetMethodName && x.GetGenericArguments().Length == genericArguments.Length);
            return targetMethod.Invoke(target, args);
         } else {
            var targetMethods = targetType.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var targetMethodMatch = targetMethods.First(x => x.Name == targetMethodName && x.GetGenericArguments().Length == genericArguments.Length);
            var targetMethod = targetMethodMatch.MakeGenericMethod(genericArguments);
            return targetMethod.Invoke(target, args);
         }
      }

      /// <summary>
      /// From morelinq 
      /// http://stackoverflow.com/questions/914109/how-to-use-linq-to-select-object-with-minimum-or-maximum-property-value
      /// </summary>
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
      public static string ToHexString(this byte[] a) {
         var hex = new StringBuilder(a.Length * 2);
         foreach (byte b in a)
            hex.AppendFormat("{0:x2}", b);
         return hex.ToString();
      }

      [Obsolete("Use ToHexString")]
      public static string ToHex(this byte[] a) => ToHexString(a);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static long ToUnixTime(this DateTime dateTime) {
         return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
      }

      [Obsolete("Use ToUnixTime")]
      public static long GetUnixTime(this DateTime dateTime) => ToUnixTime(dateTime);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static long ToUnixTimeMilliseconds(this DateTime dateTime) {
         return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
      }

      [Obsolete("Use ToUnixTimeMillis")]
      public static long GetUnixTimeMilliseconds(this DateTime dateTime) => dateTime.ToUnixTimeMilliseconds();

      //http://stackoverflow.com/questions/128618/c-file-size-format-provider
      public static string ToFileSize(this long l) {
         return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
      }

      public static async void Forget(this Task task) {
         var throwaway = task.ContinueWith(
            (t, _) => {
               if (t.IsFaulted) {
                  Console.WriteLine("Forgotten task threw: " + t.Exception);
               }
            }, null);
      }

      public static async Task Forgettable(this Task task) {
         await task.ConfigureAwait(false);
      }

      public static string ToShortString(this Guid x) {
         return x.ToString("n").Substring(0, 6);
      }
   }
}