using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface IConcurrentBag<T> : IProducerConsumerCollection<T>, IEnumerable<T>, ICollection, IEnumerable {
      void Add(T item);
      bool TryPeek(out T result);
      bool IsEmpty { get; }
   }
}