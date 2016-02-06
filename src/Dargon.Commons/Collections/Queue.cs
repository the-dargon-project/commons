using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T> {
      public Queue() { }
      public Queue(IEnumerable<T> collection) : base(collection) { }
      public Queue(int capacity) : base(capacity) { }
   }
}
