using System;
using System.Collections.Generic;
using DCC = Dargon.Commons.Collections;

namespace Dargon.Commons.Collections {
   public static class CollectionFactory {
      public static IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>() => new ConcurrentDictionary<K, V>();

      public static IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection) => new ConcurrentDictionary<K, V>(collection);

      public static IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEqualityComparer<K> comparer) => new ConcurrentDictionary<K, V>(comparer);

      public static IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection, IEqualityComparer<K> comparer) => new ConcurrentDictionary<K, V>(collection, comparer);

      public static IConcurrentSet<T> ConcurrentSet<T>() => new ConcurrentSet<T>();

      public static IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection) => new ConcurrentSet<T>(collection);

      public static IConcurrentSet<T> ConcurrentSet<T>(IEqualityComparer<T> comparer) => new ConcurrentSet<T>(comparer);

      public static IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => new ConcurrentSet<T>(collection, comparer);

      public static IConcurrentBag<T> ConcurrentBag<T>() => new ConcurrentBag<T>();

      public static IConcurrentBag<T> ConcurrentBag<T>(IEnumerable<T> collection) => new ConcurrentBag<T>(collection);

      public static IHashSet<T> HashSet<T>() => new HashSet<T>();

      public static IHashSet<T> HashSet<T>(IEnumerable<T> collection) => new HashSet<T>(collection);

      public static IHashSet<T> HashSet<T>(IEqualityComparer<T> comparer) => new HashSet<T>(comparer);

      public static IHashSet<T> HashSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) => new HashSet<T>(collection, comparer);

      public static ISortedSet<T> SortedSet<T>() => new SortedSet<T>();

      public static ISortedSet<T> SortedSet<T>(IEnumerable<T> collection) => new SortedSet<T>(collection);

      public static ISortedSet<T> SortedSet<T>(IComparer<T> comparer) => new SortedSet<T>(comparer);

      public static ISortedSet<T> SortedSet<T>(IEnumerable<T> collection, IComparer<T> comparer) => new SortedSet<T>(collection, comparer);

      public static IReadOnlyCollection<T> ImmutableCollection<T>() => DCC.ImmutableCollection.Of<T>();

      public static IReadOnlyCollection<T> ImmutableCollection<T>(params T[] args) => DCC.ImmutableCollection.Of<T>(args);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>() => DCC.ImmutableDictionary.Of<K, V>();

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1) => DCC.ImmutableDictionary.Of<K, V>(k1, v1);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9);

      public static IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9, K k10, V v10) => DCC.ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5, k6, v6, k7, v7, k8, v8, k9, v9, k10, v10);

      public static IReadOnlySet<T> ImmutableSet<T>() => DCC.ImmutableSet.Of<T>();

      public static IReadOnlySet<T> ImmutableSet<T>(params T[] args) => DCC.ImmutableSet.Of<T>(args);

      public static IMultiValueDictionary<K, V> MultiValueDictionary<K, V>() => new MultiValueDictionary<K, V>();

      public static IMultiValueDictionary<K, V> MultiValueDictionary<K, V>(IEqualityComparer<K> comparer) => new MultiValueDictionary<K, V>(comparer);

      public static IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>() => new MultiValueSortedDictionary<K, V>();

      public static IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>(IComparer<K> comparer) => new MultiValueSortedDictionary<K, V>(comparer);

      public static IOrderedDictionary<K, V> OrderedDictionary<K, V>() => new OrderedDictionary<K, V>();

      public static IOrderedDictionary<K, V> OrderedDictionary<K, V>(IEqualityComparer<K> comparer) => new OrderedDictionary<K, V>(comparer);

      public static IOrderedMultiValueDictionary<K, V> OrderedMultiValueDictionary<K, V>(ValuesSortState valuesSortState = ValuesSortState.Unsorted) => new OrderedMultiValueDictionary<K, V>(valuesSortState);

      public static IQueue<T> Queue<T>() => new Queue<T>();

      public static IPriorityQueue<TValue, TPriority> PriorityQueue<TValue, TPriority>(int capacity)
         where TPriority : IComparable<TPriority>, IEquatable<TPriority>
         where TValue : class, IPriorityQueueNode<TPriority> => new HeapPriorityQueue<TValue, TPriority>(capacity);

      public static IConcurrentQueue<T> ConcurrentQueue<T>() => new ConcurrentQueue<T>();

      public static IConcurrentQueue<T> SingleConsumerSingleProducerConcurrentQueue<T>() where T : class => new SingleConsumerSingleProducerConcurrentQueue<T>();

      public static IUniqueIdentificationSet UniqueIdentificationSet(bool filled) => new UniqueIdentificationSet(filled);

      public static IUniqueIdentificationSet UniqueIdentificationSet(uint low, uint high) => new UniqueIdentificationSet(low, high);

      public static IListDictionary<K, V> ListDictionary<K, V>() => new ListDictionary<K, V>();

      public static IDictionary<K, V> Dictionary<K, V>() => new Dictionary<K, V>();

      public static IDictionary<K, V> Dictionary<K, V>(IEqualityComparer<K> comparer) => new Dictionary<K, V>(comparer);

      public static IDictionary<K, V> SortedDictionary<K, V>() => new SortedDictionary<K, V>();

      public static IDictionary<K, V> SortedDictionary<K, V>(IComparer<K> comparer) => new SortedDictionary<K, V>(comparer);
   }
}