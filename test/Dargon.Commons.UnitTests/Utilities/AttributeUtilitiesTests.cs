using NMockito;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;

namespace Dargon.Commons.Utilities {
   public class AttributeUtilitiesTests : NMockitoInstance {
      private const string kInterfaceGuid = "8E68987F-C6F8-42CA-AE28-20CA3E8ADCED";

      [Fact]
      public void GuidlessInterface_HasNoGuid_Test() {
         Guid guid;
         var result = typeof(GuidlessInterface).TryGetInterfaceGuid(out guid);

         AssertFalse(result);
         AssertEquals(Guid.Empty, guid);
      }

      [Fact]
      public void GuidfulInterface_HasGuid_Test() {
         Console.WriteLine(typeof(GuidfulInterface).GetTypeInfo().GetCustomAttribute<GuidAttribute>());

         Guid guid;
         var result = typeof(GuidfulInterface).TryGetInterfaceGuid(out guid);

         AssertTrue(result);
         AssertEquals(Guid.Parse(kInterfaceGuid), guid);
      }

      [Fact]
      public void InheritedGuidfulInterface_HasNoGuid_Test() {
         Guid guid;
         var result = typeof(InheritedGuidfulInterface).TryGetInterfaceGuid(out guid);

         AssertFalse(result);
         AssertEquals(Guid.Empty, guid);
      }

      public interface GuidlessInterface { }

      [Guid(kInterfaceGuid)]
      public interface GuidfulInterface { }

      public interface InheritedGuidfulInterface : GuidfulInterface { }
   }
}
