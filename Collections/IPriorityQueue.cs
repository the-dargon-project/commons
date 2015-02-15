using System;

namespace ItzWarty.Collections {
   public interface IPriorityQueue<T> : IQueue<T> where T : IComparable<T> {
      /// <summary>Add an element to the priority queue - O(log(n)) time operation.</summary>
      /// <param name="item">The item to be added to the queue</param>
      void Add(T item);

      /// <summary>Returns true if the queue is empty.</summary>
      /// Trying to call Peek() or Next() on an empty queue will throw an exception.
      /// Check using Empty first before calling these methods.
      bool Empty { get; }

      bool Remove(T value);
   }
}