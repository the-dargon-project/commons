using System;
using System.Runtime.InteropServices;

namespace Dargon.Commons.Utilities {
   public class DefaultAttributeStaticsImpl : IAttributeStatics {
      public bool TryGetInterfaceGuid(Type interfaceType, out Guid guid) {
         var attribute = interfaceType.GetAttributeOrNull<GuidAttribute>();
         if (attribute == null) {
            guid = Guid.Empty;
            return false;
         } else {
            guid = Guid.Parse(attribute.Value);
            return true;
         }
      }
   }
}