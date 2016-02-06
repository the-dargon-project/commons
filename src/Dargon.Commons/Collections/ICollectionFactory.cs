using System;
using System.Collections.Generic;

namespace Dargon.Commons.Collections {
   public interface ICollectionFactory {
      IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>();
      IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection);
      IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEqualityComparer<K> comparer);
      IConcurrentDictionary<K, V> ConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection, IEqualityComparer<K> comparer);

      IConcurrentSet<T> ConcurrentSet<T>();
      IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection);
      IConcurrentSet<T> ConcurrentSet<T>(IEqualityComparer<T> comparer);
      IConcurrentSet<T> ConcurrentSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer);

      IConcurrentBag<T> ConcurrentBag<T>();
      IConcurrentBag<T> ConcurrentBag<T>(IEnumerable<T> collection);

      IHashSet<T> HashSet<T>();
      IHashSet<T> HashSet<T>(IEnumerable<T> collection);
      IHashSet<T> HashSet<T>(IEqualityComparer<T> comparer);
      IHashSet<T> HashSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer);

      ISortedSet<T> SortedSet<T>();
      ISortedSet<T> SortedSet<T>(IEnumerable<T> collection);
      ISortedSet<T> SortedSet<T>(IComparer<T> comparer);
      ISortedSet<T> SortedSet<T>(IEnumerable<T> collection, IComparer<T> comparer);

      IReadOnlyCollection<T> ImmutableCollection<T>();
      IReadOnlyCollection<T> ImmutableCollection<T>(params T[] args);

      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>();
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9);
      IReadOnlyDictionary<K, V> ImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5, K k6, V v6, K k7, V v7, K k8, V v8, K k9, V v9, K k10, V v10);

      IReadOnlySet<T> ImmutableSet<T>();
      IReadOnlySet<T> ImmutableSet<T>(params T[] args);

      IMultiValueDictionary<K, V> MultiValueDictionary<K, V>();
      IMultiValueDictionary<K, V> MultiValueDictionary<K, V>(IEqualityComparer<K> comparer);

      IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>();
      IMultiValueSortedDictionary<K, V> MultiValueSortedDictionary<K, V>(IComparer<K> comparer);

      IOrderedDictionary<K, V> OrderedDictionary<K, V>();
      IOrderedDictionary<K, V> OrderedDictionary<K, V>(IEqualityComparer<K> comparer);
         
      IOrderedMultiValueDictionary<K, V> OrderedMultiValueDictionary<K, V>(ValuesSortState valuesSortState = ValuesSortState.Unsorted);

      IQueue<T> Queue<T>();
      IPriorityQueue<TValue, TPriority> PriorityQueue<TValue, TPriority>(int capacity)
         where TPriority : IComparable<TPriority>, IEquatable<TPriority>
         where TValue : class, IPriorityQueueNode<TPriority>;
      IConcurrentQueue<T> ConcurrentQueue<T>();
      IConcurrentQueue<T> SingleConsumerSingleProducerConcurrentQueue<T>() where T : class;

      IUniqueIdentificationSet UniqueIdentificationSet(bool filled);
      IUniqueIdentificationSet UniqueIdentificationSet(uint low, uint high);

      IListDictionary<K, V> ListDictionary<K, V>();

      IDictionary<K, V> Dictionary<K, V>();
      IDictionary<K, V> Dictionary<K, V>(IEqualityComparer<K> comparer);

      IDictionary<K, V> SortedDictionary<K, V>();
      IDictionary<K, V> SortedDictionary<K, V>(IComparer<K> comparer);
   }
}
