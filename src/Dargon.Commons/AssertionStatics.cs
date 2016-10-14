using System;
using System.Diagnostics;

namespace Dargon.Commons {
   public static class AssertionStatics {
      public static void AssertEquals<T>(T expected, T actual) {
         if (!Equals(expected, actual)) {
            Fail($"AssertEquals failed. Expected: {expected}, Actual: {actual}");
         }
      }

      private static unsafe void Fail(string message) {
         Debugger.Break();
         Console.Error.WriteLine("Assertion Failure: " + message);
         Console.Error.WriteLine(Environment.StackTrace);
         Console.Error.Flush();

#if DEBUG
         Debug.Assert(false, message);
#elif TRACE
         Trace.Assert(false, message);
#else
#error Trace/Debug not defined so assertions cannot fail.
#endif
      }
   }
}
