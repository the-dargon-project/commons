using System;
using System.Collections.Generic;
using System.Linq;
using Dargon.Commons.Exceptions;

namespace Dargon.Commons
{
   public static partial class DargonCommonsExtensions
   {
      public static IEnumerable<T> Wrap<T>(this T e) {
         yield return e;
      }

      public static IEnumerable<T> Concat<T>(this IEnumerable<T> e, T value) {
         foreach (var cur in e) {
            yield return cur;
         }
         yield return value;
      }

      public static IEnumerable<T> Concat<T>(this IEnumerable<T> e, params IEnumerable<T>[] enumerables) {
         foreach (var cur in e) {
            yield return cur;
         }
         foreach (var enumerable in enumerables)
            foreach (var value in enumerable)
               yield return value;
      }

      public static T SelectRandom<T>(this IEnumerable<T> source, Random rng = null) {
         int rand = rng?.Next() ?? StaticRandom.Next(int.MaxValue);
         var options = source.ToList();
         return options[rand % options.Count];
      }

      public static T SelectRandomWeighted<T>(this IEnumerable<T> source, Func<T, int> weighter, Random rng = null) {
         int rand = rng?.Next() ?? StaticRandom.Next(int.MaxValue);
         var options = source as List<T> ?? source.ToList();
         var optionsAndWeights = options.Map(o => new { Weight = weighter(o), Option = o });
         var totalWeight = optionsAndWeights.Sum(o => o.Weight);
         if (totalWeight == 0) {
            return options[rand % options.Count];
         }
         var weightOffset = rand % totalWeight;
         foreach (var optionAndWeight in optionsAndWeights) {
            weightOffset -= optionAndWeight.Weight;
            if (weightOffset < 0) {
               return optionAndWeight.Option;
            }
         }
         throw new InvalidStateException();
      }

      // via http://stackoverflow.com/questions/1651619/optimal-linq-query-to-get-a-random-sub-collection-shuffle
      public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng = null) {
         if (source == null) throw new ArgumentNullException("source");

         return source.ShuffleIterator(rng ?? StaticRandom.NextRandom());
      }

      private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng) {
         var buffer = source.ToList();
         for (int i = 0; i < buffer.Count; i++) {
            int j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
         }
      }

      // Via http://stackoverflow.com/questions/9027530/linq-not-any-vs-all-dont
      public static bool None<TSource>(this IEnumerable<TSource> source) {
         return !source.Any();
      }

      public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
         return !source.Any(predicate);
      }

      public static string Join<T>(this IEnumerable<T> e, string delimiter) {
         return String.Join(delimiter, e);
      }

      public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Action<TAccumulate, TSource> action) {
         if (seed == null) {
            throw new ArgumentNullException("seed");
         }
         if (action == null) {
            throw new ArgumentNullException("action");
         }

         TAccumulate result = seed;
         foreach (TSource element in source) {
            action(result, element);
         }
         return result;
      }
   }
}
