using System;

namespace Dargon.Commons.Utilities {
   public static class AttributeStatics {
      private static readonly IAttributeStatics instance = new DefaultAttributeStaticsImpl();

      public static bool TryGetInterfaceGuid(this Type interfaceType, out Guid guid) {
         return instance.TryGetInterfaceGuid(interfaceType, out guid);
      }
   }
}
