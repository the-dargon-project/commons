using System;

namespace Dargon.Commons.Pooling {
   public interface IObjectPoolFactory {
      IObjectPool<T> CreateTlsBackedPool<T>(Func<T> generator);
      IObjectPool<T> CreateTlsBackedPool<T>(Func<T> generator, string name);
      IObjectPool<T> CreateStackBackedPool<T>(Func<T> generator);
      IObjectPool<T> CreateStackBackedPool<T>(Func<T> generator, string name);
   }
}
