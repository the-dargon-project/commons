using NMockito;
using System;
using Xunit;

namespace Dargon.Commons {
   public partial class UtilTests : NMockitoInstance {
      private static readonly byte[] buffer = Util.Generate(255, i => (byte)i);
      private readonly byte[] bufferCopy = Util.Generate(buffer.Length, i => buffer[i]);

      [Fact]
      public void ByteArraysEqual_HappyPathTest() {
         for (var size = 0; size < 255; size++) {
            for (var i = 0; i < buffer.Length - size + 1; i++) {
               for (var j = 0; j < bufferCopy.Length - size + 1; j++) {
                  AssertEquals(i == j || size == 0, Util.ByteArraysEqual(buffer, i, size, bufferCopy, j, size));
               }
            }
         }
      }

      [Fact]
      public void ByteArraysEqual_TrivialHappyPathTest() {
         AssertTrue(Util.ByteArraysEqual(buffer, buffer));
      }

      [Fact]
      public void ByteArraysEqual_TrivialHappyPathWithOffsetTest() {
         AssertTrue(Util.ByteArraysEqual(buffer, 0, buffer, 0, buffer.Length));
      }

      [Fact]
      public void ByteArraysEqual_TrivialSadPathTest() {
         AssertFalse(Util.ByteArraysEqual(new byte[0], new byte[1]));
      }

      [Fact]
      public void ByteArraysEqual_BoundsTest() {
         byte[] dummyBuffer = new byte[1];
         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(buffer, 1, buffer.Length, dummyBuffer, 0, 1));
         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(dummyBuffer, 0, 1, buffer, 1, buffer.Length));

         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(buffer, -1, 1, dummyBuffer, 0, 1));
         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(dummyBuffer, 0, 1, buffer, -1, 1));

         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(buffer, 1, -1, dummyBuffer, 0, 1));
         AssertThrows<IndexOutOfRangeException>(() => Util.ByteArraysEqual(dummyBuffer, 0, 1, buffer, 1, -1));
      }

      [Fact]
      public void NextToken_SimpleTest() {
         string str = "asdf qwerty  yuiop";
         string token;

         str = Tokenizer.Next(str, out token);
         AssertEquals("asdf", token);
         AssertEquals("qwerty  yuiop", str);

         str = Tokenizer.Next(str, out token);
         AssertEquals("qwerty", token);
         AssertEquals(" yuiop", str);

         str = Tokenizer.Next(str, out token);
         AssertEquals("yuiop", token);
         AssertEquals("", str);
      }

      [Fact]
      public void ToTitleCaseTests() {
         AssertEquals("", "".ToTitleCase());
         AssertEquals("", "!@#)!#@*&#".ToTitleCase());
         AssertEquals("This Is A Test", "This is a test".ToTitleCase());
         AssertEquals("Tencent Art Pack", "tencent-art-pack".ToTitleCase());
         AssertEquals("Tencent Art Pack", "tencent&#@*&!art-pack".ToTitleCase());
      }
   }
}
