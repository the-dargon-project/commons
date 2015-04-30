using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCC = System.Collections.Concurrent;

namespace ItzWarty.Collections {
   public class ConcurrentBag<T> : SCC.ConcurrentBag<T>, IConcurrentBag<T> {
      public ConcurrentBag() : base() { }
      public ConcurrentBag(IEnumerable<T> collection) : base(collection) { }
   }
}
