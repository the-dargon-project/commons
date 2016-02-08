using System;
using Dargon.Commons.Collections;
using Fody.Constructors;

namespace Dargon.Commons.Pooling {
   [RequiredFieldsConstructor]
   public class DefaultObjectPoolFactory : IObjectPoolFactory {
      private readonly ICollectionFactory collectionFactory = null;

      public IObjectPool<T> CreatePool<T>(Func<T> generator) {
         return ObjectPool.Create(generator);
      }

      public IObjectPool<T> CreatePool<T>(Func<T> generator, string name) {
         return ObjectPool.Create(generator, name);
      }
   }
}