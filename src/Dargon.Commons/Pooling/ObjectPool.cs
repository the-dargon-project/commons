﻿using System;
using Dargon.Commons.Collections;

namespace Dargon.Commons.Pooling {
   public static class ObjectPool {
      public static IObjectPool<T> Create<T>(Func<T> generator) {
         return new DefaultObjectPool<T>(generator, CollectionFactory.ConcurrentBag<T>());
      }

      public static IObjectPool<T> Create<T>(Func<T> generator, string name) {
         return new DefaultObjectPool<T>(generator, CollectionFactory.ConcurrentBag<T>(), name);
      }
   }
}