using System;
using Dargon.Commons.Collections;

namespace Dargon.Commons.Pooling {
   public class DefaultObjectPoolFactory : IObjectPoolFactory {
      private readonly ICollectionFactory collectionFactory;

      public DefaultObjectPoolFactory(ICollectionFactory collectionFactory) {
         this.collectionFactory = collectionFactory;
      }

      public IObjectPool<T> CreatePool<T>(Func<T> generator) {
         return ObjectPool.Create(generator);
      }

      public IObjectPool<T> CreatePool<T>(Func<T> generator, string name) {
         return ObjectPool.Create(generator, name);
      }
   }
}