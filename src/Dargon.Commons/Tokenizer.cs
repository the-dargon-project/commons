using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Commons {
   public static class Tokenizer {
      public static string Next(string input, out string token) {
         input = input.Trim();
         var firstSpaceIndex = input.IndexOf(' ');
         string remaining;
         if (firstSpaceIndex < 0) {
            token = input;
            remaining = "";
         } else {
            token = input.Substring(0, firstSpaceIndex);
            remaining = input.Substring(firstSpaceIndex + 1);
         }
         return remaining;
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
      public static IEnumerable<string> GetFileNameTokens(string fileName) {
         StringBuilder sb = new StringBuilder();

         // We start as if we were just at position -1
         CharType lastlastCharType = CharType.Other;
         CharType lastCharType = CharType.Other;
         CharType charType = CharType.Other;
         CharType nextCharType = fileName.Length >= 1 ? StringOperations.GetCategory(fileName[0]) : CharType.Other;
         for (int i = 0; i < fileName.Length; i++) {
            lastlastCharType = lastCharType;
            lastCharType = charType;
            charType = nextCharType;
            nextCharType = fileName.Length > i + 1 ? StringOperations.GetCategory(fileName[i + 1]) : CharType.Other;
            char c = fileName[i];
            //Console.WriteLine("Got char " + c + " current sb " + sb.ToString());

            if (sb.Length == 0) {
               if (charType != CharType.Other)
                  sb.Append(c);
            } else {
               // Check delimit condition: In the middle of two (and only two) uppercase characters that are followed by lowercase characters
               if (lastlastCharType != CharType.Uppercase && //e, current string builder = "A"
                   lastCharType == CharType.Uppercase &&     //A
                   charType == CharType.Uppercase &&         //C
                   nextCharType == CharType.Lowercase)       //a
               {
                  yield return sb.ToString();
                  sb.Clear();
                  sb.Append(c);
               } else // Check delimit condition: On switch from uppercase string of 3+ to lowercase
               if (lastlastCharType == CharType.Uppercase && //M, current string builder = "A"
                   lastCharType == CharType.Uppercase &&     //A
                   charType == CharType.Uppercase &&         //N
                   nextCharType == CharType.Lowercase)       //m
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               } else // Check delimit condition: On switch from lowercase string to uppercase
               if (charType == CharType.Lowercase &&         //n
                   nextCharType == CharType.Uppercase)       //M
               {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               } else // Check delimit condition: On switch from number to alphabet or vice versa
               if ((charType == CharType.Number && (nextCharType == CharType.Lowercase || nextCharType == CharType.Uppercase)) ||
                  (nextCharType == CharType.Number && (charType == CharType.Lowercase || charType == CharType.Uppercase))) {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               } else // Check delimit condition: On reaching a non-alphanumeric value
               if (charType == CharType.Other) {
                  if (sb.Length > 0)
                     yield return sb.ToString();
                  sb.Clear();
               } else // Check delimit condition: On reaching a non-alphanumeric value
               if (nextCharType == CharType.Other) {
                  sb.Append(c);
                  yield return sb.ToString();
                  sb.Clear();
               } else // Didn't get delimited!
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
   }

   public enum CharType { Other, Lowercase, Uppercase, Number }

   public static class StringOperations {
      public static string ToTitleCase(this string s) {
         var tokens = Tokenizer.GetFileNameTokens(s);
         return tokens.Select(ToCapitalized).Join(" ");
      }

      public static string ToCapitalized(this string token) {
         if (string.IsNullOrEmpty(token)) {
            return token;
         } else if (token.Length == 1) {
            return char.ToUpper(token[0]).ToString();
         } else {
            return char.ToUpper(token[0]) + token.Substring(1);
         }
      }

      public static CharType GetCategory(char c) {
         if ('a' <= c && c <= 'z') {
            return CharType.Lowercase;
         } else if ('A' <= c && c <= 'Z') {
            return CharType.Uppercase;
         } else if ('0' <= c && c <= '9') {
            return CharType.Number;
         } else {
            return CharType.Other;
         }
      }

      public static string Filter(this string s, Predicate<char> pass) {
         return s.Filter((x, i) => pass(x[i]));
      }

      public static string Filter(this string s, Func<string, int, bool> pass) {
         var conditions = Util.Generate(s.Length, i => pass(s, i));
         return s.LogicalIndex(conditions);
      }
   }
}
