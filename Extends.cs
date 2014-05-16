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
      /// <summary>
      /// Convers the given datetime to an HTTP timestamp, required by the HTTP Protocol
      /// </summary>
      public static string ToHTTPTimestamp(this DateTime dt)
      {
         DateTime utc = dt.ToUniversalTime();
         string day = utc.Day.ToString().MltPad0();
         string dayOfWeek = utc.DayOfWeek.ToString().Substring(0, 3);
         string year = utc.Year.ToString();
         string mon = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }[utc.Month - 1];
         return dayOfWeek + ", " + day + " " + mon + " " + year + " " + utc.Hour.ToString().MltPad0() + ":" + utc.Minute.ToString().MltPad0() + ":" + utc.Second.ToString().MltPad0() + " GMT";
      }
      /// <summary>
      /// Makes the given string length of 2, padding it to the left with a 0.
      /// </summary>
      public static string MltPad0(this string s)
      {
         if (s.Length == 2) return s;
         if (s.Length == 1) return "0" + s;
         return s;
      }

      /// <summary>
      /// (String Extension Method) convers string to bytes.
      /// </summary>
      public static byte[] ToBytes(this string s)
      {
         return System.Text.Encoding.ASCII.GetBytes(s);
      }
      /// <summary>
      /// Reverses the given string.
      /// http://dotnetperls.com/reverse-string
      /// </summary>
      public static string Reverse(this string s)
      {
         char[] arr = s.ToCharArray();
         Array.Reverse(arr);
         return new string(arr);
      }
      /// <summary>
      /// Splits the given string at the given index and returns subarrays.
      /// </summary>
      public static string[] SplitAtIndex(this string s, int index)
      {
         if (index == s.Length) return new string[] { s };
         return new string[] { s.Substring(0, index), s.Substring(index + 1) };
      }

      /// <summary>
      /// Converts the Dictionary<string, string> to an array of [key, value] elements
      /// </summary>
      public static string[][] ToStringArrayArray(this Dictionary<string, string> d)
      {
         List<string> keys = new List<string>(d.Keys);
         List<string[]> keyValues = new List<string[]>();
         for (int i = 0; i < keys.Count; i++)
         {
            keyValues.Add(new string[] { keys[i], d[keys[i]] });
         }
         return keyValues.ToArray();
      }
      /// <summary>
      /// Convers the given color to the HTML Representation of a color, #RRGGBB
      /// </summary>
      public static string ToHex(this System.Drawing.Color c)
      {
         return "#" + c.R.ToString("X").MltPad0()
                     + c.G.ToString("X").MltPad0()
                     + c.B.ToString("X").MltPad0();
      }

      /// <summary>
      /// Formats a string, shorthand for string.Format
      /// </summary>
      public static string F(this string s, params object[] p)
      {
         return string.Format(s, p);
      }

      /// <summary>
      /// Repeats the given string, s, N times
      /// </summary>
      public static string Repeat(this string s, int n)
      {
         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < n; i++)
            sb.Append(s);
         return sb.ToString();
      }

      /// <summary>
      /// Quotation aware string split.  Will not break up 'words contained in quotes'... useful for handling console
      /// such as: del "C:\Derp a de herp\Lerp a merp\"
      /// </summary>
      public static string[] QASS(this string s, char delimiter = ' ')
      {
         StringBuilder curPartSB = new StringBuilder();
         List<string> finalParts = new List<string>();
         bool inDoubleQuotes = false;
         bool inSingleQuotes = false;
         for (int i = 0; i < s.Length; i++)
         {
            if (s[i] == '"')
               if (!inSingleQuotes)
                  inDoubleQuotes = !inDoubleQuotes;
               else
                  curPartSB.Append(s[i]);
            else if (s[i] == '\'')
               if (!inDoubleQuotes)
                  inSingleQuotes = !inSingleQuotes;
               else
                  curPartSB.Append(s[i]);
            else if (s[i] == delimiter)
            {
               if (!inDoubleQuotes && !inSingleQuotes)
               {
                  if (curPartSB.ToString() != "")
                  {
                     finalParts.Add(curPartSB.ToString());
                     curPartSB.Clear();
                  }
               }
               else
               {
                  curPartSB.Append(s[i]);
               }
            }
            else
               curPartSB.Append(s[i]);
         }
         if (curPartSB.ToString() != "")
         {
            finalParts.Add(curPartSB.ToString());
         }
         return finalParts.ToArray();
      }

      /// <summary>
      /// Gets a subarray of the given array
      /// http://stackoverflow.com/questions/943635/c-arrays-getting-a-sub-array-from-an-existing-array
      /// </summary>
      public static T[] SubArray<T>(this T[] data, int index, int length)
      {
         T[] result = new T[length];
         Array.Copy(data, index, result, 0, length);
         return result;
      }

      /// <summary>
      /// Removes surrounding quotations of the given string, if they exist.
      /// </summary>
      public static string RemoveOuterQuote(this string s)
      {
         if (s.Length > 1)
         {
            if ((s[0] == '\'' && s.Last() == '\'') ||
                (s[0] == '"' && s.Last() == '"')
            )
               return s.Substring(1, s.Length - 2);
            else
               return s;
         }
         else
            return s;
      }
      /// <summary>
      /// Makes string.split() behave like JS's "string".split(delim) as opposed to c#'s requirement for StringSplitOptions
      /// The delimiter is no longer an array.
      /// </summary>
      public static string[] Split(this string s, string delimiter)
      {
         return s.Split(new string[] { delimiter }, StringSplitOptions.None);
      }
      public static string[] Split(this string s, string delimiter, StringSplitOptions sso)
      {
         return s.Split(new string[] { delimiter }, sso);
      }
      /// <summary>
      /// Converts the given byte array to a hexadecimal string representaiton of the data
      /// </summary>
      public static string ToHex(this byte[] bArray)
      {
         return BitConverter.ToString(bArray).Replace("-", "");
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      /// <summary>
      /// Gets the MD5 hash of the given string
      /// </summary>
      public static string GetMD5(this string s)
      {
         return System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(
             Encoding.ASCII.GetBytes(s)
         ).ToHex();
      }

      /// <summary>
      /// Returns whether or not a string ends with any of the given in the given array.
      /// Useful for checking if a file name ends with ".txt", ".ini", etc....
      /// </summary>
      public static bool EndsWithAny(this string s, string[] enders)
      {
         for (int i = 0; i < enders.Length; i++)
            if (s.EndsWith(enders[i])) return true;
         return false;
      }

      /// <summary>
      /// Returns whether or not a string ends with any of the given in the given array.
      /// Useful for checking if a file name ends with ".txt", ".ini", etc....
      /// </summary>
      public static bool EndsWithAny(this string s, string[] enders, StringComparison comparison)
      {
         for (int i = 0; i < enders.Length; i++)
            if (s.EndsWith(enders[i], comparison)) return true;
         return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static double GetUnixTime(this DateTime dateTime)
      {
         return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
      }
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static double GetUnixTimeMS(this DateTime dateTime)
      {
         return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
      }


      /// <summary>
      /// Reads the given amount of characters, and treats
      /// the read characters as a string.  The string should 
      /// not be null terminated.
      /// </summary>
      public static string ReadStringOfLength(this BinaryReader reader, int length)
      {
         return Encoding.ASCII.GetString(reader.ReadBytes(length));
      }

      /// <summary>
      /// Reads bytes until we get to a NULL.  Treat bytes as characters for a string.
      /// The underlying stream is treated as UTF-8.
      /// </summary>
      /// <returns></returns>
      public static string ReadNullTerminatedString(this BinaryReader reader)
      {
         List<byte> dest = new List<byte>();
         byte b;
         while ((b = reader.ReadByte()) != 0)
         {
            dest.Add(b);
         }
         return Encoding.UTF8.GetString(dest.ToArray());
      }

      public static void WriteNullTerminatedString(this BinaryWriter writer, string value)
      {
         for (int i = 0; i < value.Length; i++)
         {
            writer.Write(value[i]);
         }
         writer.Write('\0');
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static PointF ToPointF(this Point point)
      {
         return new PointF(point.X, point.Y);
      }

      //http://stackoverflow.com/questions/128618/c-file-size-format-provider
      public static string ToFileSize(this long l)
      {
         return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
      }

      public static bool ContainsAny(this string self, string[] strings, StringComparison comp = StringComparison.CurrentCulture)
      {
         foreach (var s in strings)
            if (self.IndexOf(s, comp) >= 0)
               return true;
         return false;
      }

      //http://stackoverflow.com/questions/8613187/an-elegant-way-to-consume-all-bytes-of-a-binaryreader
      public static byte[] ReadAllBytes(this BinaryReader reader)
      {
         const int bufferSize = 4096;
         using (var ms = new MemoryStream())
         {
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
               ms.Write(buffer, 0, count);
            return ms.ToArray();
         }

      }

      private delegate K TryGetValueDelegate<K, V>(K key, out V value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dict, K key)
      {
         return ((IDictionary<K, V>)dict).GetValueOrDefault(key);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dict, K key)
      {
         V result;
         dict.TryGetValue(key, out result);
         return result;
      }
      
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> dict, K key)
      {
         V result;
         dict.TryGetValue(key, out result);
         return result;
      }

      public static void Recursively<T>(this T seed, SeedInclusion seedInclusion, Action<T> action, Func<T, T> getNext)
         where T : class
      {
         var stack = new Stack<T>();
         if (seedInclusion == SeedInclusion.Include)
            stack.Push(seed);
         else
         {
            var next = getNext(seed);
            if (next != null)
               stack.Push(next);
         }

         while (stack.Any())
         {
            var node = stack.Pop();
            action(node);

            var next = getNext(node);
            if (next != null)
               stack.Push(next);
         }
      }

      public static void Recursively<T>(this T seed, SeedInclusion seedInclusion, Action<T> action, Func<T, IEnumerable<T>> getNext)
         where T : class
      {
         var stack = new Stack<T>();
         if (seedInclusion == SeedInclusion.Include)
            stack.Push(seed);
         else
         {
            var next = getNext(seed);
            foreach (var node in next.Reverse())
               stack.Push(node);
         }

         while (stack.Any())
         {
            var node = stack.Pop();
            action(node);

            var next = getNext(node);
            foreach (var following in next.Reverse())
               stack.Push(following);
         }
      }

      public static void Recursively<T>(this T seed, SeedInclusion seedInclusion, Action<T, int> action, Func<T, IEnumerable<T>> getNext)
         where T : class
      {
         var stack = new Stack<Tuple<T, int>>();
         if (seedInclusion == SeedInclusion.Include)
            stack.Push(new Tuple<T, int>(seed, 0));
         else
         {
            var next = getNext(seed);
            foreach (var node in next.Reverse())
               stack.Push(new Tuple<T, int>(node, 0));
         }

         while (stack.Any())
         {
            var node = stack.Pop();
            action(node.Item1, node.Item2);

            var next = getNext(node.Item1);
            foreach (var following in next.Reverse())
               stack.Push(new Tuple<T, int>(following, node.Item2 + 1));
         }
      }

      public static void RecursivelyReversed<T>(this T seed, SeedInclusion seedInclusion, Action<T> action, Func<T, IEnumerable<T>> getNext)
         where T : class
      {
         var stack = new Stack<T>();
         seed.Recursively(seedInclusion, stack.Push, getNext);

         while (stack.Any())
            action(stack.Pop());
      }

      public static IEnumerable<Tuple<T, IEnumerable<T>>> RecursivelyDescend<T>(
         this T seed, 
         IEnumerable<T> initAccumulator,
         AccumulatorInclusion accumulatorSeedInclusion,
         AccumulatorInclusion accumulatorTerminalInclusion, 
         Func<T, bool> terminateOnDescendent,
         Func<T, IEnumerable<T>> getDescendents)
         where T : class
      {
         if (accumulatorSeedInclusion == AccumulatorInclusion.Include)
            initAccumulator = initAccumulator.Concat(seed);

         return getDescendents(seed).Select(
            descendent => RecursivelyDescendHelper(
               descendent,
               initAccumulator, 
               accumulatorTerminalInclusion, 
               terminateOnDescendent, 
               getDescendents
            )
         ).SelectMany(many => many);
      }

      public static IEnumerable<Tuple<T, IEnumerable<T>>> RecursivelyDescendHelper<T>(
         this T seed, 
         IEnumerable<T> initAccumulator,
         AccumulatorInclusion accumulatorTerminalInclusion, 
         Func<T, bool> terminateOnDescendent,
         Func<T, IEnumerable<T>> getDescendents)
         where T : class
      {
         // ideally a linked arraylist would be used here.
         if (terminateOnDescendent(seed))
         {
            if (accumulatorTerminalInclusion == AccumulatorInclusion.Include)
               return new Tuple<T, IEnumerable<T>>(seed, initAccumulator.Concat(seed)).Wrap();
            else
               return new Tuple<T, IEnumerable<T>>(seed, initAccumulator).Wrap();
         }
         else //seed is intermediate
         {
            initAccumulator = initAccumulator.Concat(seed);
            var descendents = getDescendents(seed);
            var results = descendents.Select(
               (d) => RecursivelyDescendHelper(
                  d, 
                  initAccumulator,  
                  accumulatorTerminalInclusion, 
                  terminateOnDescendent, 
                  getDescendents
               )
            ).SelectMany(r => r);
            return results;
         }
      }

      public static IEnumerable<T> Wrap<T>(this T e)
      {
         yield return e;
      }

      public static IEnumerable<T> Concat<T>(this IEnumerable<T> e, T value)
      {
         foreach (var cur in e)
         {
            yield return cur;
         }
         yield return value;
      }

      public static IEnumerable<T> Concat<T>(this IEnumerable<T> e, params IEnumerable<T>[] enumerables)
      {
         foreach (var cur in e)
         {
            yield return cur;
         }
         foreach (var enumerable in enumerables)
            foreach(var value in enumerable)
               yield return value;
      }


      // via http://stackoverflow.com/questions/1651619/optimal-linq-query-to-get-a-random-sub-collection-shuffle
      public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng = null)
      {
         if (source == null) throw new ArgumentNullException("source");
         //if (rng == null) throw new ArgumentNullException("rng");

         return source.ShuffleIterator(rng ?? StaticRandom.NextRandom());
      }

      private static IEnumerable<T> ShuffleIterator<T>(
          this IEnumerable<T> source, Random rng)
      {
         var buffer = source.ToList();
         for (int i = 0; i < buffer.Count; i++)
         {
            int j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
         }
      }

      // Via http://stackoverflow.com/questions/9027530/linq-not-any-vs-all-dont
      public static bool None<TSource>(this IEnumerable<TSource> source)
      {
         return !source.Any();
      }

      public static bool None<TSource>(this IEnumerable<TSource> source,
                                       Func<TSource, bool> predicate)
      {
         return !source.Any(predicate);
      }

      public static bool Within(this double a, double b, double epsilon)
      {
         return Math.Abs(a - b) <= epsilon;
      }

      public static bool Within(this float a, float b, float epsilon)
      {
         return Math.Abs(a - b) <= epsilon;
      }
   }

   public enum SeedInclusion
   {
      Include,
      Exclude
   }

   public enum AccumulatorInclusion
   {
      Include,
      Exclude
   }
}
