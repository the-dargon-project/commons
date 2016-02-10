using Dargon.Commons.Collections;
using Dargon.Commons.Pooling;
using Dargon.Ryu;
using Dargon.Ryu.Modules;

namespace Dargon.Commons {
   public class DargonCommonsRyuPackage : RyuModule {
      public DargonCommonsRyuPackage() {
         Required.Singleton<DefaultCollectionFactory>().Implements<ICollectionFactory>();
         Required.Singleton<DefaultObjectPoolFactory>().Implements<IObjectPoolFactory>();
      }
   }
}
