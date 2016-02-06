using System;

namespace Dargon.Commons.Collections {
   public interface IPriorityQueue<TValue, TPriority> : IQueue<TValue> where TPriority : IComparable<TPriority> {
      void Enqueue(TValue node, TPriority priority);

      /// <summary>Returns true if the queue is empty.</summary>
      /// Trying to call Peek() or Next() on an empty queue will throw an exception.
      /// Check using Empty first before calling these methods.
      bool Empty { get; }

      bool Remove(TValue value);
   }
}
