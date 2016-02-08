using NMockito;
using Xunit;
using ICL = Dargon.Commons.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class ConcurrentDictionaryUsabilityTest : NMockitoInstance {
      readonly ICL.ConcurrentDictionary<int, string> dict = new ICL.ConcurrentDictionary<int, string>(
         new SCG.Dictionary<int, string> {
            [0] = "zero",
            [1] = "one"
         }
      );

      [Fact]
      public void ClearLacksAmbiguity() {
         dict.Clear();
      }

      [Fact]
      public void ContainsKeyLacksAmbiguity() {
         AssertTrue(dict.ContainsKey(0));
      }

      [Fact]
      public void GetEnumeratorLacksAmbiguity() {
         AssertNotNull(dict.GetEnumerator());
      }

      [Fact]
      public void TryGetValueLacksAmbiguity() {
         string value;
         dict.TryGetValue(0, out value);
         AssertEquals("zero", value);
      }

      [Fact]
      public void KeysLacksAmbiguity() {
         AssertTrue(new SCG.HashSet<int> { 0, 1 }.SetEquals(dict.Keys));
      }

      [Fact]
      public void ValuesLacksAmbiguity() {
         AssertTrue(new SCG.HashSet<string> { "zero", "one" }.SetEquals(dict.Values));
      }

      [Fact]
      public void CountLacksAmbiguity() {
         AssertEquals(2, dict.Count);
      }

      [Fact]
      public void IsEmptyLacksAmbiguity() {
         AssertFalse(dict.IsEmpty);
      }

      [Fact]
      public void IsReadOnlyLacksAmbiguity() {
         AssertFalse(dict.IsReadOnly);
      }

      [Fact]
      public void IndexerLacksAmbiguity() {
         AssertEquals("zero", dict[0]);
      }

      [Fact]
      public void ReferenceImplicitlyCastableToIReadOnlyDictionary() {
         SCG.IReadOnlyDictionary<int, string> x = dict;
         AssertEquals(x, dict);
      }

      [Fact]
      public void ReferenceImplicitlyCastableToIConcurrentDictionary() {
         ICL.IConcurrentDictionary<int, string> x = dict;
         AssertEquals(x, dict);
      }
   }
}
