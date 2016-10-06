using System.ComponentModel;

namespace Dargon.Commons {
   /// <summary>
   /// USAGE:
   /// in header:
   ///   using Console = ItzWarty.NullConsole
   ///   
   /// Stops console from being written to.
   /// </summary>
   public static class NullConsole {
      [DefaultValue(true)]
      public static bool RedirectToVoid { get; set; }

      public static string Title { get; set; }
      public static void WriteLine(params object[] asdf) {}

      public static string ReadLine() {
         return "";
      }
   }
}
