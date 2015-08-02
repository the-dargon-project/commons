using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ryu;
using ItzWarty.Collections;
using ItzWarty.Pooling;

namespace ItzWarty {
   public class ItzWartyCommonsRyuPackage : RyuPackageV1 {
      public ItzWartyCommonsRyuPackage() {
         Singleton<ICollectionFactory, CollectionFactory>();
         Singleton<ObjectPoolFactory, DefaultObjectPoolFactory>();
      }
   }
}
