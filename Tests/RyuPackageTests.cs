using Dargon.Ryu;
using ItzWarty.Collections;
using ItzWarty.Pooling;
using NMockito;
using Xunit;

namespace ItzWarty {
   public class RyuPackageTests : NMockitoInstance {
      [Fact]
      public void Run() {
         var ryu = new RyuFactory().Create();
         ryu.Touch<ItzWartyCommonsRyuPackage>();
         ryu.Setup();
         AssertTrue(ryu.Get<ICollectionFactory>() is CollectionFactory);
         AssertTrue(ryu.Get<ObjectPoolFactory>() is DefaultObjectPoolFactory);
      }
   }
}
