using System;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public class DefaultCollectionFactory : ICollectionFactory {
      public IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>() => CollectionFactory.ConcurrentDictionary<K, V>();

      public IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection) => CollectionFactory.ConcurrentDictionary<K, V>(collection);

      public IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEqualityComparer<K> comparer) => CollectionFactory.ConcurrentDictionary<K, V>(comparer);

      public IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection, IEqualityComparer<K> comparer) => CollectionFactory.ConcurrentDictionary<K, V>(collection, comparer);

      public IConcurrentSet<T> ConcurrentSet<T>() => CollectionFactory.ConcurrentSet<T>();

      public IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection) => CollectionFactory.ConcurrentSet<T>(collection);

      public IConcurrentSet<T> ConcurrentSet<T>(IEqualityComparer<T> comparer) => CollectionFactory.ConcurrentSet<T>(comparer);

      public IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => CollectionFactory.ConcurrentSet<T>(collection, comparer);

      public IConcurrentBag<T> ConcurrentBag<T>() => CollectionFactory.ConcurrentBag<T>();

      public IConcurrentBag<T> ConcurrentBag<T>(IEnumerable<T> collection) => CollectionFactory.ConcurrentBag<T>(collection);

      public IHashSet<T> HashSet<T>() => CollectionFactory.HashSet<T>();

      public IHashSet<T> HashSet<T>(IEnumerable<T> collection) => CollectionFactory.HashSet<T>(collection);

      public IHashSet<T> HashSet<T>(IEqualityComparer<T> comparer) => CollectionFactory.HashSet<T>(comparer);

      public IHashSet<T> HashSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => CollectionFactory.HashSet<T>(collection, comparer);

      public ISortedSet<T> SortedSet<T>() => CollectionFactory.SortedSet<T>();

      public ISortedSet<T> SortedSet<T>(IEnumerable<T> collection) => CollectionFactory.SortedSet<T>(collection);

      public ISortedSet<T> SortedSet<T>(IComparer<T> comparer) => CollectionFactory.SortedSet<T>(comparer);

      public ISortedSet<T> SortedSet<T>(IEnumerable<T> collection, IComparer<T> comparer) => CollectionFactory.SortedSet<T>(collection, comparer);

      public IReadOnlyCollection<T> ImmutableCollection<T>() => CollectionFactory.ImmutableCollection<T>();

      public IReadOnlyCollection<T> ImmutableCollection<T>(params T[] args) => CollectionFactory.ImmutableCollection<T>(args);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>() => CollectionFactory.ImmutableDictionary<K, V>();

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9);

      public IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9, K k10, V v10) => CollectionFactory.ImmutableDictionary<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9, k10, v10);

      public IReadOnlySet<T> ImmutableSet<T>() => CollectionFactory.ImmutableSet<T>();

      public IReadOnlySet<T> ImmutableSet<T>(params T[] args) => CollectionFactory.ImmutableSet<T>(args);

      public IMultiValueDictionary<K, V> MultiValueDictionary<K, V>() => CollectionFactory.MultiValueDictionary<K, V>();

      public IMultiValueDictionary<K, V> MultiValueDictionary<K, V>(IEqualityComparer<K> comparer) => CollectionFactory.MultiValueDictionary<K, V>(comparer);

      public IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>() => CollectionFactory.MultiValueSortedDictionary<K, V>();

      public IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>(IComparer<K> comparer) => CollectionFactory.MultiValueSortedDictionary<K, V>(comparer);

      public IOrderedDictionary<K, V> OrderedDictionary<K, V>() => CollectionFactory.OrderedDictionary<K, V>();

      public IOrderedDictionary<K, V> OrderedDictionary<K, V>(IEqualityComparer<K> comparer) => CollectionFactory.OrderedDictionary<K, V>(comparer);

      public IOrderedMultiValueDictionary<K, V> OrderedMultiValueDictionary<K, V>(ValuesSortState valuesSortState = ValuesSortState.Unsorted) => CollectionFactory.OrderedMultiValueDictionary<K, V>(valuesSortState);

      public IQueue<T> Queue<T>() => CollectionFactory.Queue<T>();

      public IPriorityQueue<TValue, TPriority> PriorityQueue<TValue, TPriority>(int capacity)
         where TPriority : IComparable<TPriority>, IEquatable<TPriority>
         where TValue : class, IPriorityQueueNode<TPriority> => CollectionFactory.PriorityQueue<TValue, TPriority>(capacity);

      public IConcurrentQueue<T> ConcurrentQueue<T>() => CollectionFactory.ConcurrentQueue<T>();

      public IConcurrentQueue<T> SingleConsumerSingleProducerConcurrentQueue<T>() where T : class => CollectionFactory.SingleConsumerSingleProducerConcurrentQueue<T>();

      public IUniqueIdentificationSet UniqueIdentificationSet(bool filled) => CollectionFactory.UniqueIdentificationSet(filled);

      public IUniqueIdentificationSet UniqueIdentificationSet(uint low, uint high) => CollectionFactory.UniqueIdentificationSet(low, high);

      public IListDictionary<K, V> ListDictionary<K, V>() => CollectionFactory.ListDictionary<K, V>();

      public IDictionary<K, V> Dictionary<K, V>() => CollectionFactory.Dictionary<K, V>();

      public IDictionary<K, V> Dictionary<K, V>(IEqualityComparer<K> comparer) => CollectionFactory.Dictionary<K, V>(comparer);

      public IDictionary<K, V> SortedDictionary<K, V>() => CollectionFactory.SortedDictionary<K, V>();

      public IDictionary<K, V> SortedDictionary<K, V>(IComparer<K> comparer) => CollectionFactory.SortedDictionary<K, V>(comparer);
   }
}