using System;
using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   /// <summary>
   /// An implementation of a min-Priority Queue using a heap.  Has O(1) .Contains()!
   /// See https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Getting%20Started for more information
   /// 
   /// A few modifications have been made by ItzWarty.
   /// </summary>
   /// <typeparam name="TValue">The values in the queue.  Must implement the PriorityQueueNode interface</typeparam>
   public sealed class HeapPriorityQueue<TValue, TPriority> : IPriorityQueue<TValue, TPriority>
       where TValue : class, IPriorityQueueNode<TPriority>
       where TPriority : IComparable<TPriority>, IEquatable<TPriority> {
      private int _numNodes;
      private readonly TValue[] _nodes;
      private long _numNodesEverEnqueued;

      /// <summary>
      /// Instantiate a new Priority Queue
      /// </summary>
      /// <param name="maxNodes">The max nodes ever allowed to be enqueued (going over this will cause an exception)</param>
      public HeapPriorityQueue(int maxNodes) {
         _numNodes = 0;
         _nodes = new TValue[maxNodes + 1];
         _numNodesEverEnqueued = 0;
      }

      public void TrimExcess() {
         // Why is this in the interface?
      }

      /// <summary>
      /// Returns the number of nodes in the queue.  O(1)
      /// </summary>
      public int Count {
         get {
            return _numNodes;
         }
      }

      public bool Empty {
         get { return _numNodes == 0; }
      }

      /// <summary>
      /// Returns the maximum number of items that can be enqueued at once in this queue.  Once you hit this number (ie. once Count == MaxSize),
      /// attempting to enqueue another item will throw an exception.  O(1)
      /// </summary>
      public int MaxSize {
         get {
            return _nodes.Length - 1;
         }
      }

      /// <summary>
      /// Removes every node from the queue.  O(n) (So, don't do this often!)
      /// </summary>
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      public void Clear() {
         for (int i = 1; i < _nodes.Length; i++)
            _nodes[i] = null;
         _numNodes = 0;
      }

      /// <summary>
      /// Returns (in O(1)!) whether the given node is in the queue.  O(1)
      /// </summary>
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      public bool Contains(TValue node) {
         return (_nodes[node.QueueIndex] == node);
      }

      public void CopyTo(TValue[] array, int arrayIndex) {
         foreach (var element in this) {
            array[arrayIndex++] = element;
         }
      }

      /// <summary>
      /// Undefined behavior as of now.
      /// Yes, this violates SOLID principles. Consider throwing a ctor parameter that takes
      /// a node and returns its priority.
      /// </summary>
      /// <param name="value"></param>
      public void Enqueue(TValue value) {
         throw new InvalidOperationException("Heap Priority Queue - Priority needed at Enqueue call");
      }

      /// <summary>
      /// Enqueue a node - .Priority must be set beforehand!  O(log n)
      /// </summary>
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      public void Enqueue(TValue node, TPriority priority) {
         node.Priority = priority;
         _numNodes++;
         _nodes[_numNodes] = node;
         node.QueueIndex = _numNodes;
         node.InsertionIndex = _numNodesEverEnqueued++;
         CascadeUp(_nodes[_numNodes]);
      }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      private void Swap(TValue node1, TValue node2) {
         //Swap the nodes
         _nodes[node1.QueueIndex] = node2;
         _nodes[node2.QueueIndex] = node1;

         //Swap their indicies
         int temp = node1.QueueIndex;
         node1.QueueIndex = node2.QueueIndex;
         node2.QueueIndex = temp;
      }

      //Performance appears to be slightly better when this is NOT inlined o_O
      private void CascadeUp(TValue node) {
         //aka Heapify-up
         int parent = node.QueueIndex / 2;
         while (parent >= 1) {
            TValue parentNode = _nodes[parent];
            if (HasHigherPriority(parentNode, node))
               break;

            //Node has lower priority value, so move it up the heap
            Swap(node, parentNode); //For some reason, this is faster with Swap() rather than (less..?) individual operations, like in CascadeDown()

            parent = node.QueueIndex / 2;
         }
      }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      private void CascadeDown(TValue node) {
         //aka Heapify-down
         TValue newParent;
         int finalQueueIndex = node.QueueIndex;
         while (true) {
            newParent = node;
            int childLeftIndex = 2 * finalQueueIndex;

            //Check if the left-child is higher-priority than the current node
            if (childLeftIndex > _numNodes) {
               //This could be placed outside the loop, but then we'd have to check newParent != node twice
               node.QueueIndex = finalQueueIndex;
               _nodes[finalQueueIndex] = node;
               break;
            }

            TValue childLeft = _nodes[childLeftIndex];
            if (HasHigherPriority(childLeft, newParent)) {
               newParent = childLeft;
            }

            //Check if the right-child is higher-priority than either the current node or the left child
            int childRightIndex = childLeftIndex + 1;
            if (childRightIndex <= _numNodes) {
               TValue childRight = _nodes[childRightIndex];
               if (HasHigherPriority(childRight, newParent)) {
                  newParent = childRight;
               }
            }

            //If either of the children has higher (smaller) priority, swap and continue cascading
            if (newParent != node) {
               //Move new parent to its new index.  node will be moved once, at the end
               //Doing it this way is one less assignment operation than calling Swap()
               _nodes[finalQueueIndex] = newParent;

               int temp = newParent.QueueIndex;
               newParent.QueueIndex = finalQueueIndex;
               finalQueueIndex = temp;
            } else {
               //See note above
               node.QueueIndex = finalQueueIndex;
               _nodes[finalQueueIndex] = node;
               break;
            }
         }
      }

      /// <summary>
      /// Returns true if 'higher' has higher priority than 'lower', false otherwise.
      /// Note that calling HasHigherPriority(node, node) (ie. both arguments the same node) will return false
      /// </summary>
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      private bool HasHigherPriority(TValue higher, TValue lower) {
         return (higher.Priority.CompareTo(lower.Priority) < 0 ||
             (higher.Priority.Equals(lower.Priority) && higher.InsertionIndex < lower.InsertionIndex));

         // return (higher.Priority < lower.Priority ||
         //     (higher.Priority == lower.Priority && higher.InsertionIndex < lower.InsertionIndex));
      }

      /// <summary>
      /// Removes the head of the queue (node with highest priority; ties are broken by order of insertion), and returns it.  O(log n)
      /// </summary>
      public TValue Dequeue() {
         TValue returnMe = _nodes[1];
         Remove(returnMe);
         return returnMe;
      }

      /// <summary>
      /// Returns the head of the queue, without removing it (use Dequeue() for that).  O(1)
      /// </summary>
      public TValue First {
         get {
            return _nodes[1];
         }
      }

      public TValue Peek() {
         return First;
      }

      public TValue[] ToArray() {
         TValue[] result = new TValue[_numNodes];
         for (int i = 1; i <= _numNodes; i++)
            result[i - 1] = _nodes[i];
         return result;
      }

      /// <summary>
      /// This method must be called on a node every time its priority changes while it is in the queue.  
      /// <b>Forgetting to call this method will result in a corrupted queue!</b>
      /// O(log n)
      /// </summary>
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
      public void UpdatePriority(TValue node, TPriority priority) {
         node.Priority = priority;
         OnNodeUpdated(node);
      }

      private void OnNodeUpdated(TValue node) {
         //Bubble the updated node up or down as appropriate
         int parentIndex = node.QueueIndex / 2;
         TValue parentNode = _nodes[parentIndex];

         if (parentIndex > 0 && HasHigherPriority(node, parentNode)) {
            CascadeUp(node);
         } else {
            //Note that CascadeDown will be called if parentNode == node (that is, node is the root)
            CascadeDown(node);
         }
      }

      /// <summary>
      /// Removes a node from the queue.  Note that the node does not need to be the head of the queue.  O(log n)
      /// </summary>
      public bool Remove(TValue node) {
         if (!Contains(node)) {
            return false;
         }

         if (_numNodes <= 1) {
            _nodes[1] = null;
            _numNodes = 0;
            return false;
         }

         //Make sure the node is the last node in the queue
         bool wasSwapped = false;
         TValue formerLastNode = _nodes[_numNodes];
         if (node.QueueIndex != _numNodes) {
            //Swap the node with the last node
            Swap(node, formerLastNode);
            wasSwapped = true;
         }

         _numNodes--;
         _nodes[node.QueueIndex] = null;

         if (wasSwapped) {
            //Now bubble formerLastNode (which is no longer the last node) up or down as appropriate
            OnNodeUpdated(formerLastNode);
         }
         return true;
      }

      public IEnumerator<TValue> GetEnumerator() {
         for (int i = 1; i <= _numNodes; i++)
            yield return _nodes[i];
      }

      IEnumerator IEnumerable.GetEnumerator() {
         return GetEnumerator();
      }

      /// <summary>
      /// <b>Should not be called in production code.</b>
      /// Checks to make sure the queue is still in a valid state.  Used for testing/debugging the queue.
      /// </summary>
      public bool IsValidQueue() {
         for (int i = 1; i < _nodes.Length; i++) {
            if (_nodes[i] != null) {
               int childLeftIndex = 2 * i;
               if (childLeftIndex < _nodes.Length && _nodes[childLeftIndex] != null && HasHigherPriority(_nodes[childLeftIndex], _nodes[i]))
                  return false;

               int childRightIndex = childLeftIndex + 1;
               if (childRightIndex < _nodes.Length && _nodes[childRightIndex] != null && HasHigherPriority(_nodes[childRightIndex], _nodes[i]))
                  return false;
            }
         }
         return true;
      }
   }

   public interface IPriorityQueueNode<TPriority> {
      /// <summary>
      /// The Priority to insert this node at.  Must be set BEFORE adding a node to the queue
      /// </summary>
      TPriority Priority { get; set; }

      /// <summary>
      /// <b>Used by the priority queue - do not edit this value.</b>
      /// Represents the order the node was inserted in
      /// </summary>
      long InsertionIndex { get; set; }

      /// <summary>
      /// <b>Used by the priority queue - do not edit this value.</b>
      /// Represents the current position in the queue
      /// </summary>
      int QueueIndex { get; set; }
   }
}