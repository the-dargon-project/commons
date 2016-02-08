using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dargon.Commons {
   public partial class UtilTests {
      private const string kDescription = "SDOKFJSOI";
      [Fact]
      public void GetAttributeOrNull_ByType_Test() {
         var attribute = typeof(DummyClass).GetAttributeOrNull<DescriptionAttribute>();
         AssertEquals(kDescription, attribute.Description);
      }

      [Description(kDescription)]
      private class DummyClass { }
   }
}
