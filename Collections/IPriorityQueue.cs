using System;

namespace ItzWarty.Collections {
   public interface IPriorityQueue<T> : IQueue<T> where T : IComparable<T> {
      /// <summary>Clear all the elements from the priority queue</summary>
      void Clear();

      /// <summary>Add an element to the priority queue - O(log(n)) time operation.</summary>
      /// <param name="item">The item to be added to the queue</param>
      void Add(T item);

      /// <summary>Returns the number of elements in the queue.</summary>
      int Count { get; }

      /// <summary>Returns true if the queue is empty.</summary>
      /// Trying to call Peek() or Next() on an empty queue will throw an exception.
      /// Check using Empty first before calling these methods.
      bool Empty { get; }

      /// <summary>Allows you to look at the first element waiting in the queue, without removing it.</summary>
      /// This element will be the one that will be returned if you subsequently call Next().
      T Peek();

      /// <summary>Removes and returns the first element from the queue (least element)</summary>
      /// <returns>The first element in the queue, in ascending order.</returns>
      T Dequeue();

      bool Remove(T value);
   }
}