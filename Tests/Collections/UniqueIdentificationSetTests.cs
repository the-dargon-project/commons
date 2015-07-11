using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMockito;
using Xunit;

namespace ItzWarty.Collections {
   public class UniqueIdentificationSetTests : NMockitoInstance {
      [Fact]
      public void __HackTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(0, 1000);
         var list = new LinkedList<UniqueIdentificationSet.Segment>();
         list.AddFirst(new UniqueIdentificationSet.Segment { low = 2000, high = 3000 });
         list.AddLast(new UniqueIdentificationSet.Segment { low = 4000, high = 5000 });
         uidSet.__Assign(list);
         list.First.Value.low = 133337; // tests clone rather than reference copy
         uidSet.__Access(x => {
            AssertEquals(2, x.Count);
            AssertEquals(2000U, x.First.Value.low);
            AssertEquals(3000U, x.First.Value.high);
            AssertEquals(4000U, x.Last.Value.low);
            AssertEquals(5000U, x.Last.Value.high);
         });
      }

      [Fact]
      public void ContainsTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(1, 4);
         uidSet.GiveRange(6, 15);
         uidSet.TakeRange(10, 11);

         AssertFalse(uidSet.Contains(0));
         AssertTrue(uidSet.Contains(1));
         AssertTrue(uidSet.Contains(2));
         AssertTrue(uidSet.Contains(3));
         AssertTrue(uidSet.Contains(4));
         AssertFalse(uidSet.Contains(5));
         AssertTrue(uidSet.Contains(6));
         AssertTrue(uidSet.Contains(7));
         AssertTrue(uidSet.Contains(8));
         AssertTrue(uidSet.Contains(9));
         AssertFalse(uidSet.Contains(10));
         AssertFalse(uidSet.Contains(11));
         AssertTrue(uidSet.Contains(12));
         AssertTrue(uidSet.Contains(13));
         AssertTrue(uidSet.Contains(14));
         AssertTrue(uidSet.Contains(15));
         AssertFalse(uidSet.Contains(16));

         AssertTrue(new UniqueIdentificationSet(0, 4).Contains(0));
      }
   }
}
