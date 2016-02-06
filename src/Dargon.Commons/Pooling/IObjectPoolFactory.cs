using System;

namespace Dargon.Commons.Pooling {
   public interface IObjectPoolFactory {
      IObjectPool<T> CreatePool<T>(Func<T> generator);
      IObjectPool<T> CreatePool<T>(Func<T> generator, string name);
   }
}
