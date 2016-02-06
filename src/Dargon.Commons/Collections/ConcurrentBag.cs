using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SCC = System.Collections.Concurrent;

namespace Dargon.Commons.Collections {
   public class ConcurrentBag<T> : SCC.ConcurrentBag<T>, IConcurrentBag<T> {
      public ConcurrentBag() : base() { }
      public ConcurrentBag(IEnumerable<T> collection) : base(collection) { }
   }
}
