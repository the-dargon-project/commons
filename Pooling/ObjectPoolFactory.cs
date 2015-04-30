using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty.Collections;

namespace ItzWarty.Pooling {
   public interface ObjectPoolFactory {
      ObjectPool<T> CreatePool<T>(Func<T> generator);
      ObjectPool<T> CreatePool<T>(Func<T> generator, string name);
   }

   public class DefaultObjectPoolFactory : ObjectPoolFactory {
      private readonly ICollectionFactory collectionFactory;

      public DefaultObjectPoolFactory(ICollectionFactory collectionFactory) {
         this.collectionFactory = collectionFactory;
      }

      public ObjectPool<T> CreatePool<T>(Func<T> generator) {
         return new ObjectPoolImpl<T>(generator, collectionFactory.CreateConcurrentBag<T>());
      }

      public ObjectPool<T> CreatePool<T>(Func<T> generator, string name) {
         return new ObjectPoolImpl<T>(generator, collectionFactory.CreateConcurrentBag<T>(), name);
      }
   }
}
