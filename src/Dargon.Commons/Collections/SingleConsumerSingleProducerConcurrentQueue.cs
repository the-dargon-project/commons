using System.Collections;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class SingleConsumerSingleProducerConcurrentQueue<T> : IConcurrentQueue<T> where T : class {
      public const int kBucketSize = 32;

      private class Segment<T> where T : class {
         public Box<T>[] elements;
         public volatile Segment<T> next; 
         public int readIndex;
         public int writeIndex;

         public Segment(Box<T>[] elements, Segment<T> next ) {
            this.elements = elements;
            this.next = next;
         }

         public class Box<T> where T : class {
            public volatile T value;
         }
      }

      private readonly Segment<T> sentinelEnd = new Segment<T>(null, null);
      private Segment<T> head;
      private Segment<T> tail;

      public SingleConsumerSingleProducerConcurrentQueue() {
         head = CreateSegment();
         tail = head;
      }

      private Segment<T> CreateSegment() {
         return new Segment<T>(Util.Generate(kBucketSize, i => new Segment<T>.Box<T>()), sentinelEnd);
      }

      public void Enqueue(T item) {
         tail.elements[tail.writeIndex++].value = item;
         if (tail.writeIndex == kBucketSize) {
            var newSegment = CreateSegment();
            tail.next = newSegment;
            tail = newSegment;
         }
      }

      public bool TryPeek(out T result) {
         AdvanceHeadPastSegmentEnd();

         var index = head.readIndex;
         if (index >= head.writeIndex) {
            result = null;
            return false;
         } else {
            result = head.elements[index].value;
            return true;
         }
      }

      public bool TryDequeue(out T result) {
         AdvanceHeadPastSegmentEnd();
         
         var index = head.readIndex;
         if (index >= head.writeIndex) {
            result = null;
            return false;
         } else {
            var element = head.elements[index];
            head.readIndex++;

            result = element.value;
            element.value = null;
            return true;
         }
      }

      private void AdvanceHeadPastSegmentEnd() {
         var index = head.readIndex;
         if (index == kBucketSize) {
            if (head.next != sentinelEnd) {
               head = head.next;
            }
         }
      }

      public void CopyTo(T[] array, int index) {
         foreach (var item in this) {
            array[index++] = item;
         }
      }

      public T[] ToArray() {
         var list = new List<T>();
         foreach (var item in this) {
            list.Add(item);
         }
         return list.ToArray();
      }

      public int Count { get { return ComputeCount(); } }
      public bool IsEmpty { get { return this.None(); } }

      private int ComputeCount() {
         var count = 0;
         var currentSegment = head;
         while (currentSegment != tail) {
            count += kBucketSize - currentSegment.readIndex;
            currentSegment = currentSegment.next;
         }
         count += tail.writeIndex;
         return count;
      }

      public IEnumerator<T> GetEnumerator() {
         var currentSegment = head;
         while (currentSegment != sentinelEnd) {
            for (var i = currentSegment.readIndex; i < kBucketSize; i++) {
               yield return currentSegment.elements[i].value;
            }
            currentSegment = currentSegment.next;
         }
      }

      IEnumerator IEnumerable.GetEnumerator() {
         return GetEnumerator();
      }
   }
}
