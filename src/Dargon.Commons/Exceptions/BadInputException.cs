using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dargon.Commons.Exceptions {
   public class BadInputException : Exception {
      public BadInputException() : base() { }
      public BadInputException(string message) : base(message) { }
   }
}
