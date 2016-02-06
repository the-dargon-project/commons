using System;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IUniqueIdentificationSet {
      /// <summary>
      /// Takes a unique identifier from the Unique Identification set.
      ///     foreach segment
      ///        if(segment.low != segment.high)
      ///          return segment.low--;
      /// </summary>
      /// <returns>A unique identifier</returns>
      uint TakeUniqueID();

      /// <summary>
      /// Takes a unique identifier from the Unique Identification set.
      /// If the UID does not exist in the set, an exception will be thrown.
      /// </summary>
      /// <param name="uid">The UID which we are taking from the set</param>
      /// <returns>A unique identifier</returns>
      void TakeUniqueID(uint uid);

      void TakeRange(uint low, uint high);

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
      void GiveUniqueID(uint value);

      void GiveRange(uint low, uint high);


      bool Any();
      int Count { get; }

      bool Contains(uint value);
      void Clear();

      IUniqueIdentificationSet Merge(IUniqueIdentificationSet mergedSet);
      IUniqueIdentificationSet Except(IUniqueIdentificationSet removedSet);
      IUniqueIdentificationSet Intersect(IUniqueIdentificationSet set);
      IUniqueIdentificationSet Invert();

      void __Assign(LinkedList<UniqueIdentificationSet.Segment> values);
      void __Access(Action<LinkedList<UniqueIdentificationSet.Segment>> accessor);
   }
}