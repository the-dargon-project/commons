using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ItzWarty
{
   public class GeneratorExitException : Exception {
      public GeneratorExitException() : base("The Generator is unable to produce more results.  Perhaps, there is nothing left to produce?") {}
   }

   public unsafe static class Util
   {
      /// <summary>
      /// Returns whether or not the given value is within (inclusive) the other two parameters
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool IsBetween(Double a, Double value, Double b)
      {
         return (a <= value && value <= b) || (b <= value && value <= a);
      }

      /// <summary>
      /// Generates a string in a stupid way...
      /// lol
      /// </summary>
      public static string GenerateString(int length)
      {
         StringBuilder temp = new StringBuilder();
         while(temp.Length < length)
            temp.Append(Guid.NewGuid().ToByteArray().ToHex());
         return temp.ToString().Substring(0, length);
      }

      /// <summary>
      /// Creates an array using the given function N times.
      /// The function takes a parameter i, from 0 to count, and returns T.
      /// </summary>
      public static T[] Generate<T>(int count, Func<int, T> generator)
      {
         if (count < 0)
            throw new ArgumentOutOfRangeException("count < 0");
         if (generator == null)
            throw new ArgumentNullException("generator");

         T[] result = new T[count];
         for (int i = 0; i < count; i++)
            result[i] = generator(i);
         return result;
      }

      /// <summary>
      /// Creates an array using the given function N times.
      /// The function takes a parameter a from 0 to countA and a parameter b, from 0 to countB, and returns T.
      /// </summary>
      public static T[] Generate<T>(int countA, int countB, Func<int, int, T> generator)
      {
         if (countA < 0)
            throw new ArgumentOutOfRangeException("countA < 0");
         if (countB < 0)
            throw new ArgumentOutOfRangeException("countB < 0");
         if (generator == null)
            throw new ArgumentNullException("generator");

         T[] result = new T[countA * countB];
         for (int a = 0; a < countA; a++)
            for (int b = 0; b < countB; b++)
               result[a * countB + b] = generator(a, b);
         return result;
      }

      /// <summary>
      /// Creates an array using the given function N times.
      /// </summary>
      public static T[] Generate<T>(int countA, int countB, int countC, Func<int, int, int, T> generator)
      {
         if (countA < 0)
            throw new ArgumentOutOfRangeException("countA < 0");
         if (countB < 0)
            throw new ArgumentOutOfRangeException("countB < 0");
         if (countC < 0)
            throw new ArgumentOutOfRangeException("countC < 0");
         if (generator == null)
            throw new ArgumentNullException("generator");

         T[] result = new T[countA * countB * countC];
         int i = 0;
         for (int a = 0; a < countA; a++)
            for (int b = 0; b < countB; b++)
               for (int c = 0; c < countC; c++)
               result[i++] = generator(a, b, c);
         return result;
      }

      /// <summary>
      /// Generates a given output.  Returns null if we are done after this loop.
      /// Throws GeneratorFinishedException if done.
      /// </summary>
      public delegate bool GeneratorDelegate<T>(int i, out T output);

      public static T[] Generate<T>(GeneratorDelegate<T> generator) where T : class
      {
         List<T> result = new List<T>();
         bool done = false;
         int i = 0;
         try
         {
            while(!done)
            {
               T output = null;
               done = generator(i++, out output);
               result.Add(output);
            }
         }
         catch(GeneratorExitException)
         {
         }
         catch(Exception e)
         {
            throw e;
         }
         return result.ToArray();
      }

      public static T[] Concat<T>(params object[] args) {
         var result = new List<T>();
         foreach (var element in args) {
            if (element is T)
               result.Add((T)element);
            else {
               foreach (var subElement in (IEnumerable<T>)element)
                  result.Add(subElement);
            }
         }
         return result.ToArray();
      }

      /// <summary>
      /// Creates a variable of the given value repeated [count] times.
      /// Note that this just copies reference if we have a Object.
      /// </summary>
      public static T[] Repeat<T>(int count, T t)
      {
         T[] result = new T[count];
         for(int i = 0; i < count; i++)
            result[i] = t;
         return result;
      }

      public static byte FindMaximum(byte[] bytes)
      {
         byte max = bytes[0];
         for(int i = 1; i < bytes.Length; i++)
         {
            if(max < bytes[i])
               max = bytes[i];
         }
         return max;
      }

      public static byte FindMinimum(byte[] bytes)
      {
         byte min = bytes[0];
         for(int i = 1; i < bytes.Length; i++)
         {
            if(min > bytes[i])
               min = bytes[i];
         }
         return min;
      }

      public static bool ByteArraysEqual(byte[] param1, byte[] param2)
      {
         if (param1.Length != param2.Length) {
            return false;
         }

         fixed (byte* pParam1 = param1)
         fixed (byte* pParam2 = param2) {
            byte* pCurrent1 = pParam1, pCurrent2 = pParam2;
            var length = param1.Length;
            int longCount = length / 8;
            for (var i = 0; i < longCount; i++) {
               if (*(ulong*)pCurrent1 != *(ulong*)pCurrent2) {
                  return false;
               }
               pCurrent1 += 8;
               pCurrent2 += 8;
            }
            if ((length & 4) != 0) {
               if (*(uint*)pCurrent1 != *(uint*)pCurrent2) {
                  return false;
               }
               pCurrent1 += 4;
               pCurrent2 += 4;
            }
            if ((length & 2) != 0) {
               if (*(ushort*)pCurrent1 != *(ushort*)pCurrent2) {
                  return false;
               }
               pCurrent1 += 2;
               pCurrent2 += 2;
            }
            if ((length & 1) != 0) {
               if (*pCurrent1 != *pCurrent2) {
                  return false;
               }
               pCurrent1 += 1;
               pCurrent2 += 1;
            }
            return true;
         }
      }

      public static void SubscribeToEventOnce<T>(ref EventHandler<T> @event, EventHandler<T> callback)
         where T : EventArgs
      {
         var signal = new CountdownEvent(2);
         var accessLock = new object();
         var done = false;
         var handler = new EventHandler<T>(
            (o, e) =>
               {
                  //Ensure no concurrent invocations of the event, though I'm not sure if .net allows for that
                  lock(accessLock)
                  {
                     //Check if we're done calling the event once.  If so, we don't want to invoke twice.
                     if(!done)
                     {
                        //We're now done.  Set the flag so we aren't called again.
                        done = true;

                        //Invoke the user's code for the one-time event subscription
                        callback(o, e);

                        //Signal that the user's code is done running, so the SubscribeToEventOnce caller
                        //thread can be unblocked.
                        signal.Signal();
                     }
                  }
               }
            );
         //Subscribe to the event which we are trying to listen to once
         @event += handler;

         //Signal the countdown event once to tell threads that we're done.  In a case like this where we're
         //really only running 1 thing at a time, it's not important.  If we had more than one thread, and were
         //trying to synchronize all of them, this would be more helpful.  For now, this allows us to
         //wait until the user code has been invoked before we allow this method to return.
         signal.Signal();

         //Wait for the user's callback event to be invoked
         signal.Wait();

         //Unsubscribe to the event.
         @event -= handler;
      }

      public class SingleSubscription
      {
         internal CountdownEvent m_countdown = new CountdownEvent(1);

         internal void Signal()
         {
            m_countdown.Signal();
         }

         public void Wait()
         {
            m_countdown.Wait();
         }
      }

      public static SingleSubscription SubscribeToEventOnceAsync<T>(Action<EventHandler<T>> subscribe,
                                                                    Action<EventHandler<T>> unsubscribe,
                                                                    EventHandler<T> callback)
         where T : EventArgs
      {
         var result = new SingleSubscription();
         var accessLock = new object();
         var done = false;
         EventHandler<T> handler = null;
         handler = new EventHandler<T>(
            (o, e) =>
               {
                  //Ensure no concurrent invocations of the event, though I'm not sure if .net allows for that
                  lock(accessLock)
                  {
                     //Check if we're done calling the event once.  If so, we don't want to invoke twice.
                     if(!done)
                     {
                        //We're now done.  Set the flag so we aren't called again.
                        done = true;

                        //Invoke the user's code for the one-time event subscription
                        callback(o, e);

                        //Signal that the user's code is done running, so the SubscribeToEventOnce caller
                        //thread can be unblocked.
                        result.Signal();

                        //Yay closures
                        unsubscribe(handler);
                     }
                  }
               }
            );
         //Subscribe to the event which we are trying to listen to once
         subscribe(handler);
         return result;
      }

      /// <SUMMARY>
      /// FROM: http://blogs.msdn.com/b/toub/archive/2006/05/05/590814.aspx
      /// Computes the Levenshtein Edit Distance between two enumerables.</SUMMARY>
      /// <TYPEPARAM name="T">The type of the items in the enumerables.</TYPEPARAM>
      /// <PARAM name="x">The first enumerable.</PARAM>
      /// <PARAM name="y">The second enumerable.</PARAM>
      /// <RETURNS>The edit distance.</RETURNS>
      public static int EditDistance<T>(IEnumerable<T> x, IEnumerable<T> y)
         where T : IEquatable<T>
      {
         // Validate parameters
         if(x == null) throw new ArgumentNullException("x");
         if(y == null) throw new ArgumentNullException("y");

         // Convert the parameters into IList instances
         // in order to obtain indexing capabilities
         IList<T> first = x as IList<T> ?? new List<T>(x);
         IList<T> second = y as IList<T> ?? new List<T>(y);

         // Get the length of both.  If either is 0, return
         // the length of the other, since that number of insertions
         // would be required.
         int n = first.Count, m = second.Count;
         if(n == 0) return m;
         if(m == 0) return n;

         // Rather than maintain an entire matrix (which would require O(n*m) space),
         // just store the current row and the next row, each of which has a length m+1,
         // so just O(m) space. Initialize the current row.
         int curRow = 0, nextRow = 1;
         int[][] rows = new int[][] {new int[m + 1], new int[m + 1]};
         for(int j = 0; j <= m; ++j) rows[curRow][j] = j;

         // For each virtual row (since we only have physical storage for two)
         for(int i = 1; i <= n; ++i)
         {
            // Fill in the values in the row
            rows[nextRow][0] = i;
            for(int j = 1; j <= m; ++j)
            {
               int dist1 = rows[curRow][j] + 1;
               int dist2 = rows[nextRow][j - 1] + 1;
               int dist3 = rows[curRow][j - 1] +
                           (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

               rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
            }

            // Swap the current and next rows
            if(curRow == 0)
            {
               curRow = 1;
               nextRow = 0;
            }
            else
            {
               curRow = 0;
               nextRow = 1;
            }
         }

         // Return the computed edit distance
         return rows[curRow][m];
      }

      /// <summary>
      /// Takes fileName like annieSquare.dds, AnnieSquare.dds, annie_square_dds, ANNIE_SQUARE.dds and 
      /// outputs  an array such as ["annie", "square", "dds"].  Non-alphanumeric values are deemed
      /// as delimiters as well.
      /// 
      /// Delimiters:
      ///    non-alphanumerics
      ///    In the middle of two (and only two) uppercase characters that are followed by lowercase characters
      ///       Ie: "ACar" => ["A", "Car"]
      ///    On switch from uppercase string of 3+ to lowercase
      ///       Ie: "MANmode" => ["MAN", "mode"]
      ///    On switch from lowercase string to uppercase
      ///       Ie: "ExampleText" => ["Example", "Text"]
      ///    On switch from number to alphabet or vice versa
      ///       Ie: "IHave10Apples" => ["I", "Have", "10", "Apples"]
      ///    On reaching a non-alphanumeric value
      ///       Ie; "RADS_USER_Kernel.exe" => ["RADS", "USER", "Kernel", "exe"]
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static IEnumerable<string> ExtractFileNameTokens(string fileName)
      {
         StringBuilder sb = new StringBuilder();

         // We start as if we were just at position -1
         CharType lastlastCharType = CharType.Invalid;
         CharType lastCharType = CharType.Invalid;
         CharType charType = CharType.Invalid;
         CharType nextCharType = fileName.Length >= 1 ? GetCharType(fileName[0]) : CharType.Invalid;
         for (int i = 0; i < fileName.Length; i++)
         {
            lastlastCharType = lastCharType;
            lastCharType = charType;
            charType = nextCharType;
            nextCharType = fileName.Length > i + 1 ? GetCharType(fileName[i + 1]) : CharType.Invalid;
            char c = fileName[i];
            //Console.WriteLine("Got char " + c + " current sb " + sb.ToString());

            if (sb.Length == 0)
            {
               if (charType != CharType.Invalid)
                  sb.Append(c);
            }
            else
            {
               // Check delimit condition: In the middle of two (and only two) uppercase characters that are followed by lowercase characters
               if (lastlastCharType != CharType.Uppercase && //e, current string builder = "A"
                   lastCharType == CharType.Uppercase &&     //A
                   charType == CharType.Uppercase &&         //C
                   nextCharType == CharType.Lowercase)       //a
               {
                  yield return sb.ToString();
                  sb.Clear();
                  sb.Append(c);
               }
               else // Check delimit condition: On switch from uppercase string of 3+ to lowercase
               if (lastlastCharType == CharType.Uppercase && //M, current string builder = "A"
                   lastCharType == CharType.Uppercase &&     //A
                   charType == CharType.Uppercase &&         //N
                   nextCharType == CharType.Lowercase)       //m
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               }
               else // Check delimit condition: On switch from lowercase string to uppercase
               if (charType == CharType.Lowercase &&         //n
                   nextCharType == CharType.Uppercase)       //M
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               }
               else // Check delimit condition: On switch from number to alphabet or vice versa
               if ((charType == CharType.Number && (nextCharType == CharType.Lowercase || nextCharType == CharType.Uppercase)) ||
                  (nextCharType == CharType.Number && (charType == CharType.Lowercase || charType == CharType.Uppercase)))
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               }
               else // Check delimit condition: On reaching a non-alphanumeric value
               if (charType == CharType.Invalid)
               {
                  if (sb.Length > 0)
                     yield return sb.ToString();
                  sb.Clear();
               }
               else // Check delimit condition: On reaching a non-alphanumeric value
               if(nextCharType == CharType.Invalid)
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               }
               else // Didn't get delimited!
               {
                  // Console.WriteLine("Appending " + c + " " + lastlastCharType + " " + lastCharType + " " + charType + " " + nextCharType);
                  sb.Append(c);
               }
            }
         } // for
         if (sb.Length > 0)
            yield return sb.ToString();
         yield break;
      }

      private static CharType GetCharType(char c)
      {
         if ('a' <= c && c <= 'z')
            return CharType.Lowercase;
         else if ('A' <= c && c <= 'Z')
            return CharType.Uppercase;
         else if ('0' <= c && c <= '9')
            return CharType.Number;
         else
            return CharType.Invalid;
      }

      private enum CharType { Invalid, Lowercase, Uppercase, Number }

      /// <summary>
      /// Formats a name from UpperCamelCase to Upper Camel Case
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static string FormatName(string name)
      {
         name = name + "   ";
         name = name[0].ToString().ToUpper() + name.Substring(1);
         //http://stackoverflow.com/questions/4511087/regex-convert-camel-case-to-all-caps-with-underscores
         string _RESULT_VAL = Regex.Replace(name, @"(?x)( [A-Z][a-z,0-9]+ | [A-Z]+(?![a-z]) )", "_$0");
         //Console.WriteLine("* " + _RESULT_VAL);
         string RESULT_VAL = _RESULT_VAL.Substring(1);
         //Console.WriteLine("# " + RESULT_VAL);

         var result = from part in RESULT_VAL.Split(new char[]{ '_', ' '})
                      let partPad = part + "  "
                      let firstChar = part.Length > 3 ? partPad[0].ToString().ToUpper() : partPad[0].ToString().ToLower()
                      select (firstChar + partPad.Substring(1).ToLower()).Trim();

         string resultString = string.Join(" ", result.Where((s)=> !string.IsNullOrWhiteSpace(s)) .ToArray()).Trim();
         
         //Make the first letter of the first term capitalized
         resultString = resultString[0].ToString().ToUpper() + resultString.Substring(1);

         //Replace multiple space occurrences
         string realResult = string.Join(" ", resultString.QASS(' '));
         //Console.WriteLine("> " + realResult);
         return realResult;
      }

      public static string RemoveNonalphanumeric(this string s)
      {
         char[] arr = s.ToCharArray();

         arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-')));
         return new string(arr);
      }

      /// <summary>
      /// http://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
      /// </summary>
      /// <param name="input"></param>
      /// <returns></returns>
      public static byte[] ReadFully(Stream input)
      {
         byte[] buffer = new byte[16 * 1024];
         using (MemoryStream ms = new MemoryStream())
         {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
               ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
         }
      }

      /// <summary>
      /// Returns an array containing numbers spaced between 0 and the given maximum value
      /// </summary>
      /// <param name="maximum">
      /// The number which the result approaches from 0 to its last index
      /// </param>
      /// <param name="numElements">
      /// The number of elements in the result (includes 0 and maximum)
      /// </param>
      /// <param name="uniformityFactor">
      /// Greater than 0
      /// </param>
      /// <param name="getRandom">Returns a value in [0.0, 1.0)</param>
      /// <returns></returns>
      public static double[] GenerateRandomCumulativeDistribution(
         double maximum, 
         int numElements, 
         double uniformityFactor,
         Func<double> getRandom)
      {
         var weights = new double[numElements];
         weights[0] = 0.0; // actually implicit, but here for readability
         for (int i = 1; i < weights.Length; i++)
            weights[i] = getRandom() + uniformityFactor;

         // :: every element equals the sum of the elements before it
         for (int i = 1; i < weights.Length; i++)
            weights[i] += weights[i - 1];

         // :: normalize all elements to maximum value keysRemaining
         for (int i = 0; i <= weights.Length - 2; i++)
            weights[i] = maximum * weights[i] / weights[weights.Length - 1];

         weights[weights.Length - 1] = maximum;
         
         return weights;
      }

      public static double[] GenerateRandomCumulativeDistribution(
         double maximum,
         int numElements,
         double uniformityFactor)
      {
         return GenerateRandomCumulativeDistribution(
            maximum, 
            numElements, 
            uniformityFactor, 
            StaticRandom.NextDouble
         );
      }

      /// <summary>
      /// Gets the attribute of Enum value
      /// </summary>
      /// <typeparam name="TAttribute"></typeparam>
      /// <param name="enumValue"></param>
      /// <returns></returns>
      public static TAttribute GetAttributeOrNull<TAttribute>(this Enum enumValue)
         where TAttribute : Attribute
      {
         var enumType = enumValue.GetType();
         var memberInfo = enumType.GetTypeInfo().DeclaredMembers.First(member => member.Name.Equals(enumValue.ToString()));
         var attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), false);
         return (TAttribute)attributes.FirstOrDefault();
      }

      public static TAttribute GetAttributeOrNull<TAttribute>(this object instance)
         where TAttribute : Attribute
      {
         var attributes = instance.GetType().GetTypeInfo().GetCustomAttributes(typeof(TAttribute), false);
         return (TAttribute)attributes.FirstOrDefault();
      }

      public static bool IsThrown<TException>(Action action) where TException : Exception {
         try {
            action();
            return false;
         } catch (TException) {
            return true;
         }
      }

      public static TValue KeepExisting<TKey, TValue>(TKey key, TValue value) {
         return value;
      }

      public static long GetUnixTimeMilliseconds() {
         return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
      }
   }
}
