using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections {
   public class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T> {
      public Queue() { }
      public Queue(IEnumerable<T> collection) : base(collection) { }
      public Queue(int capacity) : base(capacity) { }
   }
}
