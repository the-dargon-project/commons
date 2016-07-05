using System;
using Dargon.Commons.Collections;

namespace Dargon.Commons.Pooling {
   public static class ObjectPool {
      public static IObjectPool<T> CreateTlsBacked<T>(Func<T> generator) {
         return CreateTlsBacked<T>(pool => generator());
      }

      public static IObjectPool<T> CreateTlsBacked<T>(Func<IObjectPool<T>, T> generator) {
         return new TlsBackedObjectPool<T>(generator);
      }

      public static IObjectPool<T> CreateTlsBacked<T>(Func<T> generator, string name) {
         return CreateTlsBacked<T>(pool => generator(), name);
      }

      public static IObjectPool<T> CreateTlsBacked<T>(Func<IObjectPool<T>, T> generator, string name) {
         return new TlsBackedObjectPool<T>(generator, name);
      }

      public static IObjectPool<T> CreateStackBacked<T>(Func<T> generator) {
         return CreateStackBacked<T>(pool => generator());
      }

      public static IObjectPool<T> CreateStackBacked<T>(Func<IObjectPool<T>, T> generator) {
         return new StackBackedObjectPool<T>(generator);
      }

      public static IObjectPool<T> CreateStackBacked<T>(Func<T> generator, string name) {
         return CreateStackBacked<T>(pool => generator(), name);
      }

      public static IObjectPool<T> CreateStackBacked<T>(Func<IObjectPool<T>, T> generator, string name) {
         return new StackBackedObjectPool<T>(generator, name);
      }
   }
}