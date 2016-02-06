using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class ConcurrentQueue<T> : System.Collections.Concurrent.ConcurrentQueue<T>, IConcurrentQueue<T> {
      public ConcurrentQueue() { }
      public ConcurrentQueue(IEnumerable<T> collection) : base(collection) { }
   }
}
