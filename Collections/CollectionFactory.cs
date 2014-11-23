using System;
using System.Collections.Generic;

namespace ItzWarty.Collections {
   public class CollectionFactory : ICollectionFactory {
      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>() {
         return new ConcurrentDictionary<K, V>();
      }

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection) {
         return new ConcurrentDictionary<K, V>(collection);
      }

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEqualityComparer<K> comparer) {
         return new ConcurrentDictionary<K, V>(comparer);
      }

      public IConcurrentDictionary<K, V> CreateConcurrentDictionary<K, V>(IEnumerable<KeyValuePair<K, V>> collection, IEqualityComparer<K> comparer) {
         return new ConcurrentDictionary<K, V>(collection, comparer);
      }

      public IConcurrentSet<T> CreateConcurrentSet<T>() {
         return new ConcurrentSet<T>();
      }

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEnumerable<T> collection) {
         return new ConcurrentSet<T>(collection);
      }

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEqualityComparer<T> comparer) {
         return new ConcurrentSet<T>(comparer);
      }

      public IConcurrentSet<T> CreateConcurrentSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) {
         return new ConcurrentSet<T>(collection, comparer);
      }

      public IHashSet<T> CreateHashSet<T>() {
         return new HashSet<T>();
      }

      public IHashSet<T> CreateHashSet<T>(IEnumerable<T> collection) {
         return new HashSet<T>(collection);
      }

      public IHashSet<T> CreateHashSet<T>(IEqualityComparer<T> comparer) {
         return new HashSet<T>(comparer);
      }

      public IHashSet<T> CreateHashSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) {
         return new HashSet<T>(collection, comparer);
      }

      public ISortedSet<T> CreateSortedSet<T>() {
         return new SortedSet<T>();
      }

      public ISortedSet<T> CreateSortedSet<T>(IEnumerable<T> collection) {
         return new SortedSet<T>(collection);
      }

      public ISortedSet<T> CreateSortedSet<T>(IComparer<T> comparer) {
         return new SortedSet<T>(comparer);
      }

      public ISortedSet<T> CreateSortedSet<T>(IEnumerable<T> collection, IComparer<T> comparer) {
         return new SortedSet<T>(collection, comparer);
      }

      public IReadOnlyCollection<T> CreateImmutableCollection<T>() {
         return ImmutableCollection.Of<T>();
      }

      public IReadOnlyCollection<T> CreateImmutableCollection<T>(params T[] args) {
         return ImmutableCollection.Of<T>(args);
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>() {
         return ImmutableDictionary.Of<K, V>();
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1) {
         return ImmutableDictionary.Of<K, V>(k1, v1);
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2) {
         return ImmutableDictionary.Of<K, V>(k1, v1, k2, v2);
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3) {
         return ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3);
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4) {
         return ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4);
      }

      public IReadOnlyDictionary<K, V> CreateImmutableDictionary<K, V>(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) {
         return ImmutableDictionary.Of<K, V>(k1, v1, k2, v2, k3, v3, k4, v4, k5, v5);
      }

      public IReadOnlySet<T> CreateImmutableSet<T>() {
         return ImmutableSet.Of<T>();
      }

      public IReadOnlySet<T> CreateImmutableSet<T>(params T[] args) {
         return ImmutableSet.Of<T>(args);
      }

      public IMultiValueDictionary<K, V> CreateMultiValueDictionary<K, V>() {
         return new MultiValueDictionary<K, V>();
      }

      public IMultiValueDictionary<K, V> CreateMultiValueDictionary<K, V>(IEqualityComparer<K> comparer) {
         return new MultiValueDictionary<K, V>(comparer);
      }

      public IMultiValueSortedDictionary<K, V> CreateMultiValueSortedDictionary<K, V>() {
         return new MultiValueSortedDictionary<K, V>();
      }

      public IMultiValueSortedDictionary<K, V> CreateMultiValueSortedDictionary<K, V>(IComparer<K> comparer) {
         return new MultiValueSortedDictionary<K, V>(comparer);
      }

      public IOrderedDictionary<K, V> CreateOrderedDictionary<K, V>() {
         return new OrderedDictionary<K, V>();
      }

      public IOrderedDictionary<K, V> CreateOrderedDictionary<K, V>(IEqualityComparer<K> comparer) {
         return new OrderedDictionary<K, V>(comparer);
      }

      public IOrderedMultiValueDictionary<K, V> CreateOrderedMultiValueDictionary<K, V>(ValuesSortState valuesSortState = ValuesSortState.Unsorted) {
         return new OrderedMultiValueDictionary<K, V>(valuesSortState);
      }

      public IQueue<T> CreateQueue<T>() {
         return new Queue<T>();
      }

      public IPriorityQueue<T> CreatePriorityQueue<T>() where T : IComparable<T> {
         return new PriorityQueue<T>();
      }

      public IUniqueIdentificationSet CreateUniqueIdentificationSet(bool filled) {
         return new UniqueIdentificationSet(filled);
      }

      public IUniqueIdentificationSet CreateUniqueIdentificationSet(uint low, uint high) {
         return new UniqueIdentificationSet(low, high);
      }

      public IListDictionary<K, V> CreateListDictionary<K, V>() {
         return new ListDictionary<K, V>();
      }

      public IDictionary<K, V> CreateDictionary<K, V>() {
         return new Dictionary<K, V>();
      }

      public IDictionary<K, V> CreateDictionary<K, V>(IEqualityComparer<K> comparer) {
         return new Dictionary<K, V>(comparer);
      }

      public IDictionary<K, V> CreateSortedDictionary<K, V>() {
         return new SortedDictionary<K, V>();
      }

      public IDictionary<K, V> CreateSortedDictionary<K, V>(IComparer<K> comparer) {
         return new SortedDictionary<K, V>(comparer);
      }
   }
}