using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dargon.Commons.Exceptions {
   public class InvalidStateException : Exception {
      public InvalidStateException() : base() { }
      public InvalidStateException(string message) : base(message) { }
   }
}
