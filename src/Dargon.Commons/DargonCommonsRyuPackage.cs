using Dargon.Commons.Collections;
using Dargon.Commons.Pooling;
using Dargon.Ryu;

namespace Dargon.Commons {
   public class DargonCommonsRyuPackage : RyuPackageV1 {
      public DargonCommonsRyuPackage() {
         Singleton<ICollectionFactory, DefaultCollectionFactory>();
         Singleton<IObjectPoolFactory, DefaultObjectPoolFactory>();
      }
   }
}
