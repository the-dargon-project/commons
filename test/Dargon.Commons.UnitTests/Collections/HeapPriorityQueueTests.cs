using NMockito;
using Xunit;

namespace Dargon.Commons.Collections {
// Ripped from https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/src/19dda413a12d87e61fa819280f33e25534f2b4e8/Priority%20Queue%20Tests/HeapPriorityQueueTests.cs?at=master - License MIT
   public class HeapPriorityQueueTests : NMockitoInstance {
      private class Node : IPriorityQueueNode<int> {
         public Node(int priority) {
            Priority = priority;
         }

         public long InsertionIndex { get; set; }
         public int Priority { get; set; }
         public int QueueIndex { get; set; }
      }

      [Fact]
      public void TestSanity() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);

         AssertEquals(node1, node1);
         AssertEquals(node2, node2);
         AssertFalse(node1.Equals(node2));
      }

      [Fact]
      public void TestSimpleQueue() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node2);
         Enqueue(queue, node5);
         Enqueue(queue, node1);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      }
      
      [Fact]
      public void TestForwardOrder() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node1);
         Enqueue(queue, node2);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
         Enqueue(queue, node5);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      }
      
      [Fact]
      public void TestBackwardOrder() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node5);
         Enqueue(queue, node4);
         Enqueue(queue, node3);
         Enqueue(queue, node2);
         Enqueue(queue, node1);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      }
      
      [Fact]
      public void TestAddingSameNodesLater() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node2);
         Enqueue(queue, node5);
         Enqueue(queue, node1);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      
         Enqueue(queue, node5);
         Enqueue(queue, node3);
         Enqueue(queue, node1);
         Enqueue(queue, node2);
         Enqueue(queue, node4);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      }
      
      [Fact]
      public void TestAddingDifferentNodesLater() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node2);
         Enqueue(queue, node5);
         Enqueue(queue, node1);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      
         Node node6 = new Node(6);
         Node node7 = new Node(7);
         Node node8 = new Node(8);
         Node node9 = new Node(9);
         Node node10 = new Node(10);
      
         Enqueue(queue, node6);
         Enqueue(queue, node7);
         Enqueue(queue, node8);
         Enqueue(queue, node10);
         Enqueue(queue, node9);
      
         AssertEquals(node6, Dequeue(queue));
         AssertEquals(node7, Dequeue(queue));
         AssertEquals(node8, Dequeue(queue));
         AssertEquals(node9, Dequeue(queue));
         AssertEquals(node10, Dequeue(queue));
      }
      
      [Fact]
      public void TestClear() {
         Node node1 = new Node(1);
         Node node2 = new Node(2);
         Node node3 = new Node(3);
         Node node4 = new Node(4);
         Node node5 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node2);
         Enqueue(queue, node5);
         queue.Clear();
         Enqueue(queue, node1);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
      }
      
      [Fact]
      public void TestOrderedQueue() {
         Node node1 = new Node(1);
         Node node2 = new Node(1);
         Node node3 = new Node(1);
         Node node4 = new Node(1);
         Node node5 = new Node(1);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(5);
         Enqueue(queue, node1);
         Enqueue(queue, node2);
         Enqueue(queue, node3);
         Enqueue(queue, node4);
         Enqueue(queue, node5);
      
         AssertEquals(node1, Dequeue(queue));
         AssertEquals(node2, Dequeue(queue));
         AssertEquals(node3, Dequeue(queue));
         AssertEquals(node4, Dequeue(queue));
         AssertEquals(node5, Dequeue(queue));
      }
      
      [Fact]
      public void TestMoreComplicatedQueue() {
         Node node11 = new Node(1);
         Node node12 = new Node(1);
         Node node13 = new Node(1);
         Node node14 = new Node(1);
         Node node15 = new Node(1);
         Node node21 = new Node(2);
         Node node22 = new Node(2);
         Node node23 = new Node(2);
         Node node24 = new Node(2);
         Node node25 = new Node(2);
         Node node31 = new Node(3);
         Node node32 = new Node(3);
         Node node33 = new Node(3);
         Node node34 = new Node(3);
         Node node35 = new Node(3);
         Node node41 = new Node(4);
         Node node42 = new Node(4);
         Node node43 = new Node(4);
         Node node44 = new Node(4);
         Node node45 = new Node(4);
         Node node51 = new Node(5);
         Node node52 = new Node(5);
         Node node53 = new Node(5);
         Node node54 = new Node(5);
         Node node55 = new Node(5);
      
         HeapPriorityQueue<Node, int> queue = new HeapPriorityQueue<Node, int>(25);
         Enqueue(queue, node31);
         Enqueue(queue, node51);
         Enqueue(queue, node52);
         Enqueue(queue, node11);
         Enqueue(queue, node21);
         Enqueue(queue, node22);
         Enqueue(queue, node53);
         Enqueue(queue, node41);
         Enqueue(queue, node12);
         Enqueue(queue, node32);
         Enqueue(queue, node13);
         Enqueue(queue, node42);
         Enqueue(queue, node43);
         Enqueue(queue, node44);
         Enqueue(queue, node45);
         Enqueue(queue, node54);
         Enqueue(queue, node14);
         Enqueue(queue, node23);
         Enqueue(queue, node24);
         Enqueue(queue, node33);
         Enqueue(queue, node34);
         Enqueue(queue, node55);
         Enqueue(queue, node35);
         Enqueue(queue, node25);
         Enqueue(queue, node15);
      
         AssertEquals(node11, Dequeue(queue));
         AssertEquals(node12, Dequeue(queue));
         AssertEquals(node13, Dequeue(queue));
         AssertEquals(node14, Dequeue(queue));
         AssertEquals(node15, Dequeue(queue));
         AssertEquals(node21, Dequeue(queue));
         AssertEquals(node22, Dequeue(queue));
         AssertEquals(node23, Dequeue(queue));
         AssertEquals(node24, Dequeue(queue));
         AssertEquals(node25, Dequeue(queue));
         AssertEquals(node31, Dequeue(queue));
         AssertEquals(node32, Dequeue(queue));
         AssertEquals(node33, Dequeue(queue));
         AssertEquals(node34, Dequeue(queue));
         AssertEquals(node35, Dequeue(queue));
         AssertEquals(node41, Dequeue(queue));
         AssertEquals(node42, Dequeue(queue));
         AssertEquals(node43, Dequeue(queue));
         AssertEquals(node44, Dequeue(queue));
         AssertEquals(node45, Dequeue(queue));
         AssertEquals(node51, Dequeue(queue));
         AssertEquals(node52, Dequeue(queue));
         AssertEquals(node53, Dequeue(queue));
         AssertEquals(node54, Dequeue(queue));
         AssertEquals(node55, Dequeue(queue));
      }
      
      private void Enqueue(HeapPriorityQueue<Node, int> queue, Node node) {
         queue.Enqueue(node, node.Priority);
         AssertTrue(queue.IsValidQueue());
      }
      
      private Node Dequeue(HeapPriorityQueue<Node, int> queue) {
         Node returnMe = queue.Dequeue();
         AssertTrue(queue.IsValidQueue());
         return returnMe;
      }
   }
}