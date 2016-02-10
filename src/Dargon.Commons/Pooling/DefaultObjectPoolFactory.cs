using Fody.Constructors;
using System;

namespace Dargon.Commons.Pooling {
   [RequiredFieldsConstructor]
   public class DefaultObjectPoolFactory : IObjectPoolFactory {
      public IObjectPool<T> CreatePool<T>(Func<T> generator) {
         return ObjectPool.Create(generator);
      }

      public IObjectPool<T> CreatePool<T>(Func<T> generator, string name) {
         return ObjectPool.Create(generator, name);
      }
   }
}