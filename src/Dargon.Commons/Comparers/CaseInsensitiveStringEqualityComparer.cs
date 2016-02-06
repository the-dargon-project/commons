using System;
using System.Collections.Generic;

namespace Dargon.Commons.Comparers {
   public unsafe class CaseInsensitiveStringEqualityComparer : IEqualityComparer<string> {
      public bool Equals(string x, string y) {
         return x.Equals(y, StringComparison.OrdinalIgnoreCase);
      }

      // hashpjw
      public int GetHashCode(string obj) {
         uint hash = 5381;
         fixed (char* pString = obj) {
            uint c;
            char* s = pString;
            while ((c = s[0]) != 0) {
               // tolower the character
               if (c >= (uint)'A' && c <= (uint)'Z') {
                  c += 32;
               }

               hash = (hash << 4) + c;
               uint g = hash & 0xF0000000U;
               if (g != 0) {
                  hash = hash ^ (g >> 24);
                  hash = hash ^ g;
               }
               s++;
            }
         }
         return unchecked((int)hash);
      }
   }
}
