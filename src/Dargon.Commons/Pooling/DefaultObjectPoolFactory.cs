using System;

namespace Dargon.Commons.Pooling {
   public class DefaultObjectPoolFactory : IObjectPoolFactory {
      public IObjectPool<T> CreateTlsBackedPool<T>(Func<T> generator) {
         return ObjectPool.CreateTlsBacked(generator);
      }

      public IObjectPool<T> CreateTlsBackedPool<T>(Func<T> generator, string name) {
         return ObjectPool.CreateTlsBacked(generator, name);
      }

      public IObjectPool<T> CreateStackBackedPool<T>(Func<T> generator) {
         return ObjectPool.CreateStackBacked(generator);
      }

      public IObjectPool<T> CreateStackBackedPool<T>(Func<T> generator, string name) {
         return ObjectPool.CreateStackBacked(generator, name);
      }
   }
}