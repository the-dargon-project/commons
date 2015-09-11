using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ItzWarty.Utilities {
   public static class AttributeUtilities {
      private static readonly AttributeUtilitiesInterface instance = new AttributeUtilitiesImpl();

      public static bool TryGetInterfaceGuid(Type interfaceType, out Guid guid) {
         return instance.TryGetInterfaceGuid(interfaceType, out guid);
      }
   }

   public interface AttributeUtilitiesInterface {
      bool TryGetInterfaceGuid(Type interfaceType, out Guid guid);
   }

   public class AttributeUtilitiesImpl : AttributeUtilitiesInterface {
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
