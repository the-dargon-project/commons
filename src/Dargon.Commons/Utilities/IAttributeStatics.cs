using System;

namespace Dargon.Commons.Utilities {
   public interface IAttributeStatics {
      bool TryGetInterfaceGuid(Type interfaceType, out Guid guid);
   }
}