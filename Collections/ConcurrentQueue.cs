using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ItzWarty.Collections {
   public class ConcurrentQueue<T> : System.Collections.Concurrent.ConcurrentQueue<T>, IConcurrentQueue<T> {
      public ConcurrentQueue() { }
      public ConcurrentQueue(IEnumerable<T> collection) : base(collection) { }
   }
}
