using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMockito;
using Xunit;

namespace Dargon.Commons.Collections {
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
      public void GiveRange_AppendToBackTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.GiveRange(1, 5);
         uidSet.GiveRange(7, 10);
         uidSet.GiveRange(12, 15);
         uidSet.GiveRange(18, 20);
         AssertEquals("[1, 5][7, 10][12, 15][18, 20]", uidSet.ToString());
      }

      [Fact]
      public void GiveRange_PrependToFrontTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.Clear();
         uidSet.GiveRange(18, 20);
         uidSet.GiveRange(12, 15);
         uidSet.GiveRange(7, 10);
         uidSet.GiveRange(1, 5);
         AssertEquals("[1, 5][7, 10][12, 15][18, 20]", uidSet.ToString());
      }

      [Fact]
      public void GiveRange_AppendInMiddleTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.GiveRange(12, 15);
         uidSet.GiveRange(18, 20);
         uidSet.GiveRange(1, 5);
         uidSet.GiveRange(7, 10);
         AssertEquals("[1, 5][7, 10][12, 15][18, 20]", uidSet.ToString());
      }

      [Fact]
      public void GiveRange_MergeAfterFirstTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.GiveRange(1, 5);
         uidSet.GiveRange(10, 12);
         uidSet.GiveRange(8, 9);
         AssertEquals("[1, 5][8, 12]", uidSet.ToString());
      }

      [Fact]
      public void GiveRange_MergeMultipleTest() {
         var hardList = new LinkedList<UniqueIdentificationSet.Segment>().With(list => {
            list.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 12, high = 15 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 30, high = 50 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 52, high = 100 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 150, high = 200 });
         });
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.__Assign(hardList);
         uidSet.GiveRange(0, 16);
         AssertEquals("[0, 16][30, 50][52, 100][150, 200]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.GiveRange(15, 51);
         AssertEquals("[1, 5][12, 100][150, 200]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.GiveRange(15, UInt32.MaxValue);
         AssertEquals("[1, 5][12, " + UInt32.MaxValue + "]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.GiveRange(0, UInt32.MaxValue);
         AssertEquals("[0, " + UInt32.MaxValue + "]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.GiveRange(201, 1000);
         AssertEquals("[1, 5][12, 15][30, 50][52, 100][150, 1000]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.GiveRange(0, 0);
         AssertEquals("[0, 5][12, 15][30, 50][52, 100][150, 200]", uidSet.ToString());
      }

      [Fact]
      public void TakeRangeStartTest() {
         var hardList = new LinkedList<UniqueIdentificationSet.Segment>().With(list => {
            list.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 12, high = 15 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 30, high = 50 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 52, high = 100 });
            list.AddLast(new UniqueIdentificationSet.Segment { low = 150, high = 200 });
         });
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         uidSet.__Assign(hardList);
         uidSet.TakeRange(3, 11);
         AssertEquals("[1, 2][12, 15][30, 50][52, 100][150, 200]", uidSet.ToString());

         uidSet.__Assign(hardList);
         uidSet.TakeRange(0, 300);
         AssertEquals("", uidSet.ToString());
       
         uidSet.__Assign(hardList);
         uidSet.TakeRange(1, 3);
         AssertEquals("[4, 5][12, 15][30, 50][52, 100][150, 200]", uidSet.ToString()); 

         uidSet.__Assign(hardList);
         uidSet.TakeRange(1, 5);
         AssertEquals("[12, 15][30, 50][52, 100][150, 200]", uidSet.ToString()); 

         uidSet.__Assign(hardList);
         uidSet.TakeRange(150, 200);
         AssertEquals("[1, 5][12, 15][30, 50][52, 100]", uidSet.ToString()); 
      }

      [Fact]
      public void IntersectTest() {
         var setA = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 16 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 18, high = 22 });
         })));
         var setB = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 2 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 4, high = 6 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 9, high = 12 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 15, high = 20 });
         })));
         AssertEquals("[1, 2][4, 5][10, 12][15, 16][18, 20]", setA.Intersect(setB).ToString());
      }

      [Fact]
      public void InvertTest() {
         var setA = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 0, high = 5 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 16 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 18, high = 22 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 25, high = 100 });
         })));
         var setB = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 16 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 18, high = 22 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 25, high = 100 });
         })));
         var setC = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 16 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 18, high = 22 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 25, high = uint.MaxValue });
         })));
         var setD = new UniqueIdentificationSet(false).With(set => set.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(x => {
            x.AddLast(new UniqueIdentificationSet.Segment { low = 0, high = 5 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 16 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 18, high = 22 });
            x.AddLast(new UniqueIdentificationSet.Segment { low = 25, high = uint.MaxValue });
         })));
         AssertEquals("[6, 9][17, 17][23, 24][101, " + UInt32.MaxValue + "]", setA.Invert().ToString());
         AssertEquals("[0, 0][6, 9][17, 17][23, 24][101, " + UInt32.MaxValue + "]", setB.Invert().ToString());
         AssertEquals("[0, 0][6, 9][17, 17][23, 24]", setC.Invert().ToString());
         AssertEquals("[6, 9][17, 17][23, 24]", setD.Invert().ToString());
      }

      [Fact]
      public void InvertBoundsTest() {
         AssertEquals("", new UniqueIdentificationSet(true).Invert().ToString());
         AssertEquals("[0, " + UInt32.MaxValue + "]", new UniqueIdentificationSet(false).Invert().ToString());
      }

      [Fact]
      public void GiveTakeMergeExceptInvertIntersectRangeRandomTest() {
         var random = new Random();
         var set = new HashSet<uint>();
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(false);
         for (var it = 0; it < 20000; it++) {
            var low = (uint)(random.NextDouble() * (long)100000);
            var high = low + (uint)(random.NextDouble() * (long)100);

            if (random.Next() % 2 == 0) {
               for (var val = low; val <= high; val++) {
                  AssertEquals(set.Add(val), !uidSet.Contains(val));
               }
               if (random.Next() % 2 == 0) {
                  uidSet.GiveRange(low, high);
               } else {
                  uidSet = uidSet.Merge(new UniqueIdentificationSet(low, high).Invert().Invert());
               }
               for (var val = low; val <= high; val++) {
                  AssertTrue(uidSet.Contains(val));
               }
            } else {
               for (var val = low; val <= high; val++) {
                  AssertEquals(set.Remove(val), uidSet.Contains(val));
               }
               var rand = random.Next() % 3;
               if (rand == 0) {
                  uidSet.TakeRange(low, high);
               } else if (rand == 1) {
                  uidSet = uidSet.Except(new UniqueIdentificationSet(low, high).Invert().Invert());
               } else {
                  uidSet = uidSet.Intersect(new UniqueIdentificationSet(low, high).Invert());
               }
               for (var val = low; val <= high; val++) {
                  AssertFalse(uidSet.Contains(val));
               }
            }
         }
      }

      [Fact]
      public void MergeTest () {
         IUniqueIdentificationSet first = new UniqueIdentificationSet(false).With(x => {
            x.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(list => {
               list.AddLast(new UniqueIdentificationSet.Segment { low = 1, high = 5 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 10, high = 12 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 15, high = 20 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 22, high = 30 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 40, high = UInt32.MaxValue - 1 });
            }));
         });
         IUniqueIdentificationSet second = new UniqueIdentificationSet(false).With(x => {
            x.__Assign(new LinkedList<UniqueIdentificationSet.Segment>().With(list => {
               list.AddLast(new UniqueIdentificationSet.Segment { low = 0, high = 0 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 6, high = 7 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 13, high = 14 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 20, high = 21 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = 22, high = 30 });
               list.AddLast(new UniqueIdentificationSet.Segment { low = UInt32.MaxValue, high = UInt32.MaxValue });
            }));
         });
         AssertEquals(first.Merge(second).ToString(), "[0, 7][10, 30][40, " + UInt32.MaxValue + "]");
      }

      [Fact]
      public void Any_TrivialTest() {
         AssertEquals(0, new UniqueIdentificationSet(false).Count);
         AssertEquals(3, new UniqueIdentificationSet(0, 2).Count);
      }

      [Fact]
      public void Any_ComplexTest() {
         IUniqueIdentificationSet uidSet = new UniqueIdentificationSet(1, 2);
         uidSet.GiveRange(4, 5);

         AssertEquals(4, uidSet.Count);
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
