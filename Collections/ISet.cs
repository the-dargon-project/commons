using System.Collections;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlySet<T> {
   }
}
