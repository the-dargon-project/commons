using NMockito;
using Xunit;
using ICL = ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace ItzWarty.Collections {
   public class PriorityQueueUsabilityTest : NMockitoInstance {
      private readonly ICL.PriorityQueue<int> testObj = new PriorityQueue<int>().With(x => {
         x.Add(1);
         x.Add(2);
      });

      [Fact]
      public void ClearLacksAmbiguity() {
         testObj.Clear();
      }

      [Fact]
      public void CountLacksAmbiguity() {
         AssertEquals(2, testObj.Count);
      }

      [Fact]
      public void PeekLacksAmbiguity() {
         AssertEquals(1, testObj.Peek());
      }

      [Fact]
      public void DequeueLacksAmbiguity() {
         AssertEquals(1, testObj.Dequeue());
      }
   }
}
