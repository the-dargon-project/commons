using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Commons.Channels {
   public interface Channel<T> : ReadableChannel<T>, WritableChannel<T> { }

   public interface ReadableChannel<T> {
      Task<T> ReadAsync(CancellationToken cancellationToken, Func<T, bool> acceptanceTest);
   }

   public interface WritableChannel<T> {
      Task WriteAsync(T message, CancellationToken cancellationToken);
   }
}
