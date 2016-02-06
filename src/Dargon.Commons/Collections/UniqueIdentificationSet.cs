using System;
using System.Collections.Generic;
using System.Text;

namespace Dargon.Commons.Collections
{
   /// <summary>
   /// Represents a set of all unique UInt32 values.
   /// Values can be taken out and returned to the set in a thread-safe manner.
   /// 
   /// Implementation details:
   /// We efficiently implement the UID Set through a segment-based system.
   /// We begin with a segment [0, UINT32_MAX], and when the user requests a new Unique Identifier,
   /// we find the leftmost segment which has size != 1.  That segment is chopped, resulting in the
   /// following segment and non-included value: [1,UINT32_MAX] 0
   /// 
   /// We can continue to efficiently chop this segment: [10,UINT32_MAX] 0 1 2 3 4 5 6 7 8 9
   /// Let's try to return a value (5) back into the set, now.
   /// This is done in an O(N) operation as well; we scan from left to right until we find a segment
   /// which starts with a value greater than us. We insert before that segment.
   /// [5,5][10, UINT32_MAX} 0 1 2 3 4 6 7 8 9
   /// 
   /// Let's return another number, 9.  We learn of another rule: If we are 1-SEGMENT.MIN, then we
   /// update the segment's minimum value.  If we are 1=SEGMENT.MAX, then we update the segment's 
   /// maximum value, in an in-place fashion. The implementation should be careful of overflows when
   /// performing such an operation.
   /// 
   /// If we find ourselves incremeting the end of a segment, we must look at the segment which 
   /// follows (such a segment should always exist unless we have UINT32_MAX freed values).  If our
   /// new MAX_VALUE is equivalent to that segment's MIN_VALUE, our segments must be joined.
   /// 
   /// Overall:
   ///   Initial State: [0, UINT32_MAX]
   /// 10 UIDs removed: [10, UINT32_MAX}        0 1 2 3 4 5 6 7 8 9
   ///  UID 7 returned: [7,7][10, UINT32_MAX]   0 1 2 3 4 5 6   8 9
   ///  UID 6 returned: [6,7][10, UINT32_MAX]   0 1 2 3 4 5     8 9
   ///  UID 9 returned: [6,7][ 9, UINT32_MAX]   0 1 2 3 4 5     8
   ///  UID 8 returned: [6, UINT32_MAX]         0 1 2 3 4 5
   /// 
   /// Logical Steps:
   ///   GetUniqueID():
   ///     foreach segment
   ///        if(segment.low != segment.high)
   ///          return segment.low++;
   ///   ReturnUniqueID(value):
   ///     foreach segment
   ///        if(segment.low == value + 1) //This ensures we don't face overflow issues
   ///          segment.low = value;
   ///        else if(segment.high = value - 1)
   ///        {
   ///          segment.high = value;
   ///          if(nextSegment.low = segment.high)
   ///          {
   ///            segment.high = nextSegment.high;
   ///            RemoveSegment(nextSegment).
   ///          }
   ///        }
   ///        else if(segment.low > value) // Ie: Inserting 3 before [5, INT32_MAX]
   ///        {
   ///          segment.prepend([value, value]);
   ///        }
   /// </summary>
   public class UniqueIdentificationSet : IUniqueIdentificationSet {
      /// <summary>
      /// A segment in our UID Set
      /// <seealso cref="UniqueIdentificationSet"/>
      /// </summary>
      public class Segment
      {
         /// <summary>
         /// The inclusive low end of our segment's range
         /// </summary>
         public uint low;

         /// <summary>
         /// The inclusive high end of our segment's range
         /// </summary>
         public uint high;

         public Segment Clone() {
            return new Segment { low = low, high = high };
         }
      }

      /// <summary>
      /// Our linkedlist of segments
      /// </summary>
      private readonly LinkedList<Segment> m_segments;

      /// <summary>
      /// We use this lock object to ensure that only one thread modifies the m_segments list at a time.
      /// </summary>
      private object m_lock = new object();

      /// <summary>
      /// Initializes a new instance of a Unique Identification Set as either filled or empty
      /// </summary>
      /// <param name="filled">
      /// If set to true, the set is initially full.  Otherwise, the set is initially empty.
      /// </param>
      public UniqueIdentificationSet(bool filled)
      {
         m_segments = new LinkedList<Segment>();
         if (filled)
            m_segments.AddFirst(new Segment() { low = 0, high = UInt32.MaxValue });
      }

      /// <summary>
      /// Initializes a new instance of a Unique Identification Set with the given initial range of
      /// available values
      /// </summary>
      /// <param name="low">The inclusive low bound of the set</param>
      /// <param name="high">The inclusive high bound of the set</param>
      public UniqueIdentificationSet(uint low, uint high)
      {
         m_segments = new LinkedList<Segment>();
         m_segments.AddFirst(new Segment() { low = low, high = high});
      }

      public int Count => ComputeCount();

      /// <summary>
      /// Takes a unique identifier from the Unique Identification set.
      ///     foreach segment
      ///        if(segment.low != segment.high)
      ///          return segment.low--;
      /// </summary>
      /// <returns>A unique identifier</returns>
      public uint TakeUniqueID()
      {
         lock (m_lock)
         {
            var it = m_segments.First;
            while (it != null)
            {
               if (it.Value.low != it.Value.high)
                  return it.Value.low++;
               it = it.Next;
            }
            throw new Exception("The Unique ID Set was unable to supply a new Unique ID.  Check for UID Leaks");
         }
      }

      /// <summary>
      /// Takes a unique identifier from the Unique Identification set.
      /// If the UID does not exist in the set, an exception will be thrown.
      /// </summary>
      /// <param name="uid">The UID which we are taking from the set</param>
      /// <returns>A unique identifier</returns>
      public void TakeUniqueID(uint uid)
      {
         lock (m_lock)
         {
            var it = m_segments.First;
            bool done = false;
            while (it != null && !done)
            {
               Segment segment = it.Value;
               if (segment.low == segment.high)
               {
                  if (uid == segment.low) //And thusly uid equals segment.high
                  {
                     m_segments.Remove(segment);
                     done = true;
                  }
               }
               else
               {
                  if (uid == segment.low)
                  {
                     segment.low++;
                     done = true;
                  }
                  else if (uid == segment.high)
                  {
                     segment.high--;
                     done = true;
                  }
                  else if (segment.low < uid && uid < segment.high)
                  {
                     Segment newSegment = new Segment() { low = uid + 1, high = segment.high };
                     segment.high = uid - 1;
                     m_segments.AddAfter(it, newSegment);
                     done = true;
                  }
               }
               it = it.Next;
            }
            if(!done)
               throw new Exception("The Unique ID Set was unable to take the given Unique ID.  Check for UID Leaks");
         }
      }

      /// <summary>
      /// Returns a unique identifier to the Unique Identification Set.
      ///     foreach segment
      ///        if(segment.low == value + 1) //This ensures we don't face overflow issues
      ///          segment.low = value;
      ///        else if(segment.high = value - 1)
      ///        {
      ///          segment.high = value;
      ///          if(nextSegment.low = segment.high)
      ///          {
      ///            segment.high = nextSegment.high;
      ///            RemoveSegment(nextSegment).
      ///          }
      ///        }
      ///        else if(segment.low > value) // Ie: Inserting 3 before [5, INT32_MAX]
      ///        {
      ///          segment.prepend([value, value]);
      ///        }
      /// </summary>
      /// <param name="value">The UID which we are returning to the set.</param>
      public void GiveUniqueID(uint value)
      {
         lock (m_lock)
         {
            Segment segment;
            LinkedListNode<Segment> neighborNode;
            var it = m_segments.First;
            if (it == null) //We have an empty set
            {
               m_segments.AddFirst(new Segment() { low = value, high = value });
            }
            else
            {
               bool done = false;
               while (it != null && !done)
               {
                  segment = it.Value;
                  if (segment.low == value + 1 && value != UInt32.MaxValue)
                  {
                     segment.low = value;
                     neighborNode = it.Previous;
                     if (neighborNode != null && neighborNode.Value.high == segment.low + 1)
                     {
                        segment.low = neighborNode.Value.low;
                        m_segments.Remove(neighborNode);
                     }
                     done = true;
                  }
                  else if (segment.high == value - 1 && value != UInt32.MinValue)
                  {
                     segment.high = value;
                     neighborNode = it.Next;
                     if (neighborNode != null && neighborNode.Value.low - 1 == segment.high)
                     {
                        segment.high = neighborNode.Value.high;
                        m_segments.Remove(neighborNode);
                     }
                     done = true;
                  }
                  else if (segment.low > value)
                  {
                     Segment newSegment = new Segment() { low = value, high = value };
                     m_segments.AddBefore(it, newSegment);
                     done = true;
                  }
                  else if (segment.high < value && it.Next == null)
                  {
                     Segment newSegment = new Segment() { low = value, high = value };
                     m_segments.AddAfter(it, newSegment);
                     done = true;
                  }
                  else if (segment.low == value || segment.high == value)
                  {
                     throw new Exception("Attempted to return UID to UID Set, but value already existed in set!");
                  }
                  else
                  {
                     it = it.Next;
                     done = false;
                  }
               }
               if (!done)
                  throw new Exception("Unable to return UID to Unique ID Set, check for duplicate returns");
            }
         }
      }

      public void TakeRange(uint low, uint high) {
         // [1,5][7,10][12,15][30,50][50,60] given [6,30], [7,30]
         // start: ^
         //   end:               ^
         lock (m_lock) {
            if (m_segments.Count == 0) {
               // There's nothing to remove.
               return;
            } else if (m_segments.Count > 0) {
               // Removal before the front
               if (m_segments.First.Value.low > high) {
                  return;
               }
               
               // Removal afer the back
               if (m_segments.Last.Value.high < low) {
                  return;
               }
            }

            // The start segment is the smallest sector that contains or is greater-than low.
            var startSegment = m_segments.First;
            while (startSegment != null && startSegment.Value.high < low) {
               startSegment = startSegment.Next;
            }

            // Find end segment, greatest segment that contains or is less than low.
            var endSegment = startSegment;
            if (high == UInt32.MaxValue) {
               endSegment = m_segments.Last;
            } else {
               while (endSegment.Next != null && endSegment.Next.Value.low <= high + 1) {
                  endSegment = endSegment.Next;
               }
            }

            if (startSegment == endSegment) {
               var isLowerBoundCut = startSegment.Value.low >= low;
               var isUpperBoundCut = startSegment.Value.high <= high;
               if (isLowerBoundCut && isUpperBoundCut) {
                  m_segments.Remove(startSegment);
               } else if (isLowerBoundCut && !isUpperBoundCut) {
                  startSegment.Value.low = Math.Max(startSegment.Value.low, high + 1);
               } else if (!isLowerBoundCut && isUpperBoundCut) {
                  startSegment.Value.high = Math.Min(startSegment.Value.high, low - 1);
               } else {
                  m_segments.AddAfter(startSegment, new Segment { low = high + 1, high = startSegment.Value.high });
                  startSegment.Value.high = low - 1;
               }
            } else {
               // Delete everything between startSegment and endSegment
               while (startSegment.Next != endSegment) {
                  m_segments.Remove(startSegment.Next);
               }

               if (startSegment.Value.low >= low) {
                  m_segments.Remove(startSegment);
               } else {
                  startSegment.Value.high = low - 1;
               }

               if (endSegment.Value.high <= high) {
                  m_segments.Remove(endSegment);
               } else {
                  endSegment.Value.low = high + 1;
               }
            }
         }
      }

      public void GiveRange(uint low, uint high) {
         // [1,5][7,10][12,15][30,50][55,60] given [9,35] or [11,52] or [6,30]
         // start:  ^
         //   end:               ^
         //
         // [1,5][12,15][30,50][52,100][150,200]
         lock (m_lock) {
            // Edge cases... Kill me x_x
            if (m_segments.Count == 0) {
               // Edge case: The list is empty
               m_segments.AddLast(new LinkedListNode<Segment>(new Segment { low = low, high = high }));
               return;
            } else if (m_segments.Count > 0) {
               // Edge case: start-of-list insertion
               if (m_segments.First.Value.low > high + 1 && high != UInt32.MaxValue) {
                  m_segments.AddFirst(new LinkedListNode<Segment>(new Segment { low = low, high = high }));
                  return;
               }

               // Edge case: start of list merge
               if (m_segments.First.Value.low == high + 1 && high != UInt32.MaxValue) {
                  m_segments.First.Value.low = low;
                  return;
               }

               // Edge case: end-of-list insertion
               if (m_segments.Last.Value.high < low - 1 && low != 0) {
                  m_segments.AddLast(new LinkedListNode<Segment>(new Segment { low = low, high = high }));
                  return;
               }

               // Edge case: end-of-list merge
               if (m_segments.Last.Value.high == low - 1 && low != 0) {
                  m_segments.Last.Value.high = high;
                  return;
               }
            }

            // [1,5][12,15][30,50][52,100][150,200]
            // Find start segment, which we're either expanding or adding before
            // In terms of properties, smallest segment that contains, is mergeable with, or is greater than low.
            var startSegment = m_segments.First;
            while (startSegment != null && startSegment.Value.high < low - 1 && low != 0) {
               startSegment = startSegment.Next;
            }

            // Find end segment, greatest segment that contains, is mergeable with, or is less than low.
            var endSegment = startSegment;
            if (high == UInt32.MaxValue) {
               endSegment = m_segments.Last;
            } else {
               while (endSegment.Next != null && endSegment.Next.Value.low <= high + 1) {
                  endSegment = endSegment.Next;
               }
            }

            if (startSegment == endSegment) {
               if (startSegment.Value.low > high + 1) {
                  // prepend
                  m_segments.AddBefore(startSegment, new Segment { low = low, high = high });
               } else {
                  startSegment.Value.low = Math.Min(low, startSegment.Value.low);
                  startSegment.Value.high = Math.Max(high, startSegment.Value.high);
               }
            } else {
               startSegment.Value.low = Math.Min(low, startSegment.Value.low);
               startSegment.Value.high = Math.Max(high, endSegment.Value.high);
               
               // Delete everything from startSegment to and including endSegment
               while (startSegment.Next != endSegment) {
                  m_segments.Remove(startSegment.Next);
               }
               m_segments.Remove(startSegment.Next);
            }
         }
      }

      public IUniqueIdentificationSet Merge(IUniqueIdentificationSet otherInput) {
         // clone other to prevent deadlock
         UniqueIdentificationSet other = new UniqueIdentificationSet(false);
         otherInput.__Access(other.__Assign);

         var results = new LinkedList<Segment>();
         other.__Access(otherSegments => {
            lock (m_lock) {
               Segment currentSegment = null;
               foreach (var segment in MergeHelper_OrderSegmentsByLower(m_segments, otherSegments)) {
                  if (currentSegment == null) {
                     currentSegment = segment;
                  } else {
                     if (currentSegment.high != UInt32.MaxValue && currentSegment.high + 1 >= segment.low) {
                        currentSegment.high = Math.Max(currentSegment.high, segment.high);
                     } else {
                        results.AddLast(currentSegment);
                        currentSegment = new Segment { low = segment.low, high = segment.high };
                     }
                  }
               }
               if (currentSegment != null) {
                  results.AddLast(currentSegment);
               }
            }
         });
         return new UniqueIdentificationSet(false).With(uidSet => uidSet.__Assign(results));
      }

      private IEnumerable<Segment> MergeHelper_OrderSegmentsByLower(LinkedList<Segment> a, LinkedList<Segment> b) {
         var aCurrent = a.First;
         var bCurrent = b.First;
         while(aCurrent != null && bCurrent != null) {
            if (aCurrent.Value.low <= bCurrent.Value.low) {
               yield return aCurrent.Value;
               aCurrent = aCurrent.Next;
            } else {
               yield return bCurrent.Value;
               bCurrent = bCurrent.Next;
            }
         }
         while (aCurrent != null) {
            yield return aCurrent.Value;
            aCurrent = aCurrent.Next;
         }
         while (bCurrent != null) {
            yield return bCurrent.Value;
            bCurrent = bCurrent.Next;
         }
      }

      public IUniqueIdentificationSet Except(IUniqueIdentificationSet removedSetInput) {
         var result = new UniqueIdentificationSet(false).With(x => this.__Access(x.__Assign));
         var removedSet = new UniqueIdentificationSet(false).With(x => removedSetInput.__Access(x.__Assign));
         removedSet.__Access(removedSegments => {
            foreach (var segment in removedSegments) {
               result.TakeRange(segment.low, segment.high);
            }
         });
         return result;
      }

      public IUniqueIdentificationSet Intersect(IUniqueIdentificationSet setInput) {
         // [1     5]   [10        16]  [18  22]
         // [12] [4 6] [9   12]  [15      20]
         var set = new UniqueIdentificationSet(false).With(x => setInput.__Access(x.__Assign));
         lock (m_lock) {
            var resultList = new LinkedList<Segment>();
            var leftCurrent = m_segments.First;
            var rightCurrent = set.m_segments.First;
            while (leftCurrent != null && rightCurrent != null) {
               var intersects = !(leftCurrent.Value.high < rightCurrent.Value.low ||
                                  leftCurrent.Value.low > rightCurrent.Value.high);
               if (intersects) {
                  resultList.AddLast(new Segment {
                     low = Math.Max(leftCurrent.Value.low, rightCurrent.Value.low),
                     high = Math.Min(leftCurrent.Value.high, rightCurrent.Value.high)
                  });
               }

               if (leftCurrent.Value.high < rightCurrent.Value.high) {
                  leftCurrent = leftCurrent.Next;
               } else {
                  rightCurrent = rightCurrent.Next;
               }
            }
            return new UniqueIdentificationSet(false).With(x => x.__Assign(resultList));
         }
      }

      public IUniqueIdentificationSet Invert() {
         lock (m_lock) {
            // Trivial case: empty set -> full set
            if (m_segments.Count == 0) {
               return new UniqueIdentificationSet(true);
            }

            var resultList = new LinkedList<Segment>();
            var node = m_segments.First;
            if (node.Value.low != 0) {
               resultList.AddLast(new Segment { low = 0, high = node.Value.low - 1 });
            }
            while (node.Next != null) {
               resultList.AddLast(new Segment { low = node.Value.high + 1, high = node.Next.Value.low - 1 });
               node = node.Next;
            }
            if (node.Value.high != UInt32.MaxValue) {
               resultList.AddLast(new Segment { low = node.Value.high + 1, high = UInt32.MaxValue });
            }
            return new UniqueIdentificationSet(false).With(x => x.__Assign(resultList));
         }
      }

      public bool Any() {
         lock (m_lock) {
            return m_segments.Count > 0;
         }
      }

      private int ComputeCount() {
         lock (m_lock) {
            int count = 0;
            foreach (var segment in m_segments) {
               count += (int)(segment.high - segment.low + 1);
            }
            return count;
         }
      }

      public bool Contains(uint value) {
         lock (m_lock) {
            // [1 4] [6 10] [13 15]
            var node = m_segments.First;
            while(node != null && value > node.Value.high) {
               node = node.Next;
            }
            return node != null && node.Value.low <= value;
         }
      }

      public void Clear() {
         lock (m_lock) {
            m_segments.Clear();
         }
      }

      public void __Assign(LinkedList<Segment> values) {
         lock (m_lock) {
            this.m_segments.Clear();
            foreach (var value in values) {
               m_segments.AddLast(new Segment { low = value.low, high = value.high });
            }
         }
      }

      public void __Access(Action<LinkedList<Segment>> accessor) {
         lock (m_lock) {
            accessor(m_segments);
         }
      }

      /// <summary>
      /// Returns a string representation of the UID set, useful for debugging
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();
         foreach (Segment segment in m_segments)
         {
            sb.Append("[" + segment.low + ", " + segment.high + "]");
         }
         return sb.ToString();
      }
   }
}
