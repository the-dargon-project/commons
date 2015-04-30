using System;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public class CollectionFactory : ICollectionFactory {
      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>() => new ConcurrentDictionary<K, V>();

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection) => new ConcurrentDictionary<K, V>(collection);

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEqualityComparer<K> comparer) => new ConcurrentDictionary<K, V>(comparer);

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection, IEqualityComparer<K> comparer) => new ConcurrentDictionary<K, V>(collection, comparer);

      public IConcurrentSet<T> CreateConcurrentSet<T>() => new ConcurrentSet<T>();

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEnumerable<T> collection) => new ConcurrentSet<T>(collection);

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEqualityComparer<T> comparer) => new ConcurrentSet<T>(comparer);

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => new ConcurrentSet<T>(collection, comparer);

      public IConcurrentBag<T> CreateConcurrentBag<T>() => new ConcurrentBag<T>();

      public IConcurrentBag<T> CreateConcurrentBag<T>(IEnumerable<T> collection) => new ConcurrentBag<T>(collection);

      public IHashSet<T> CreateHashSet<T>() => new HashSet<T>();

      public IHashSet<T> CreateHashSet<T>(IEnumerable<T> collection) => new HashSet<T>(collection);

      public IHashSet<T> CreateHashSet<T>(IEqualityComparer<T> comparer) => new HashSet<T>(comparer);

      public IHashSet<T> CreateHashSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => new HashSet<T>(collection, comparer);

      public ISortedSet<T> CreateSortedSet<T>() => new SortedSet<T>();

      public ISortedSet<T> CreateSortedSet<T>(IEnumerable<T> collection) => new SortedSet<T>(collection);

      public ISortedSet<T> CreateSortedSet<T>(IComparer<T> comparer) => new SortedSet<T>(comparer);

      public ISortedSet<T> CreateSortedSet<T>(IEnumerable<T> collection, IComparer<T> comparer) => new SortedSet<T>(collection, comparer);

      public IReadOnlyCollection<T> CreateImmutableCollection<T>() => ImmutableCollection.Of<T>();

      public IReadOnlyCollection<T> CreateImmutableCollection<T>(params T[] args) => ImmutableCollection.Of<T>(args);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>() => ImmutableDictionary.Of<K, V>();

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1) => ImmutableDictionary.Of<K, V>(k1, v1);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9);

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9, K k10, V v10) => ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9, k10, v10);

      public IReadOnlySet<T> CreateImmutableSet<T>() => ImmutableSet.Of<T>();

      public IReadOnlySet<T> CreateImmutableSet<T>(params T[] args) => ImmutableSet.Of<T>(args);

      public IMultiValueDictionary<K, V> CreateMultiValueDictionary<K, V>() => new MultiValueDictionary<K, V>();

      public IMultiValueDictionary<K, V> CreateMultiValueDictionary<K, V>(IEqualityComparer<K> comparer) => new MultiValueDictionary<K, V>(comparer);

      public IMultiValueSortedDictionary<K, V> CreateMultiValueSortedDictionary<K, V>() => new MultiValueSortedDictionary<K, V>();

      public IMultiValueSortedDictionary<K, V> CreateMultiValueSortedDictionary<K, V>(IComparer<K> comparer) => new MultiValueSortedDictionary<K, V>(comparer);

      public IOrderedDictionary<K, V> CreateOrderedDictionary<K, V>() => new OrderedDictionary<K, V>();

      public IOrderedDictionary<K, V> CreateOrderedDictionary<K, V>(IEqualityComparer<K> comparer) => new OrderedDictionary<K, V>(comparer);

      public IOrderedMultiValueDictionary<K, V> CreateOrderedMultiValueDictionary<K, V>(ValuesSortState valuesSortState = ValuesSortState.Unsorted) => new OrderedMultiValueDictionary<K, V>(valuesSortState);

      public IQueue<T> CreateQueue<T>() => new Queue<T>();

      public IPriorityQueue<TValue, TPriority> CreatePriorityQueue<TValue, TPriority>(int capacity)
         where TPriority : IComparable<TPriority>, IEquatable<TPriority>
         where TValue : class, IPriorityQueueNode<TPriority> => new HeapPriorityQueue<TValue, TPriority>(capacity);

      public IConcurrentQueue<T> CreateConcurrentQueue<T>() => new ConcurrentQueue<T>();

      public IConcurrentQueue<T> CreateSingleConsumerSingleProducerConcurrentQueue<T>() where T : class => new SingleConsumerSingleProducerConcurrentQueue<T>();

      public IUniqueIdentificationSet CreateUniqueIdentificationSet(bool filled) => new UniqueIdentificationSet(filled);

      public IUniqueIdentificationSet CreateUniqueIdentificationSet(uint low, uint high) => new UniqueIdentificationSet(low, high);

      public IListDictionary<K, V> CreateListDictionary<K, V>() => new ListDictionary<K, V>();

      public IDictionary<K, V> CreateDictionary<K, V>() => new Dictionary<K, V>();

      public IDictionary<K, V> CreateDictionary<K, V>(IEqualityComparer<K> comparer) => new Dictionary<K, V>(comparer);

      public IDictionary<K, V> CreateSortedDictionary<K, V>() => new SortedDictionary<K, V>();

      public IDictionary<K, V> CreateSortedDictionary<K, V>(IComparer<K> comparer) => new SortedDictionary<K, V>(comparer);
   }
}