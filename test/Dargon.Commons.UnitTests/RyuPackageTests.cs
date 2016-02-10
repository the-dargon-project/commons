using Dargon.Ryu;
using Dargon.Commons.Collections;
using Dargon.Commons.Pooling;
using NMockito;
using Xunit;

namespace Dargon.Commons {
   public class RyuPackageTests : NMockitoInstance {
      [Fact]
      public void Run() {
         var ryu = new RyuFactory().Create();
         AssertTrue(ryu.GetOrThrow<ICollectionFactory>() is DefaultCollectionFactory);
         AssertTrue(ryu.GetOrThrow<IObjectPoolFactory>() is DefaultObjectPoolFactory);
      }
   }
}
