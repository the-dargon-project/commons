﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Collections
{
   public class ListDictionary<K, V> : IDictionary<K, V>
   {
      private List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>();  

      public IEnumerator<KeyValuePair<K, V>> GetEnumerator() { return list.GetEnumerator(); }
      IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

      public V Get(K key) { return list.First(kvp => kvp.Key.Equals(key)).Value; }

      public void AddOrUpdate(K key, V value)
      {
         bool updated = false;
         for (var i = 0; i < list.Count; i++) {
            if (Equals(key, list[i].Key)) {
               list[i] = new KeyValuePair<K, V>(key, value);
               updated = true;
            }
         }
         if (!updated) {
            list.Add(new KeyValuePair<K, V>(key, value));
         }
      }

      /// <summary>
      /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
      public void Add(KeyValuePair<K, V> item) {
         if (ContainsKey(item.Key))
         {
            throw new ArgumentException("Key already existed!");
         }
         else
         {
            list.Add(item);
         }
      }

      /// <summary>
      /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
      public void Clear() { list.Clear(); }

      /// <summary>
      /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
      /// </summary>
      /// <returns>
      /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
      /// </returns>
      /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
      public bool Contains(KeyValuePair<K, V> item) { return list.Contains(item); }

      /// <summary>
      /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
      /// </summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
      public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }

      /// <summary>
      /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <returns>
      /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </returns>
      /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
      public bool Remove(KeyValuePair<K, V> item) { return list.Remove(item); }

      /// <summary>
      /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </summary>
      /// <returns>
      /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
      /// </returns>
      public int Count { get { return list.Count; } }

      /// <summary>
      /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
      /// </summary>
      /// <returns>
      /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
      /// </returns>
      public bool IsReadOnly { get { return false; } }

      /// <summary>
      /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
      /// </summary>
      /// <returns>
      /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
      /// </returns>
      /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
      public bool ContainsKey(K key) {
         foreach (var kvp in list) {
            if (Equals(key, kvp.Key))
            {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <param name="key">The object to use as the key of the element to add.</param><param name="value">The object to use as the value of the element to add.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public void Add(K key, V value) { Add(new KeyValuePair<K, V>(key, value));}

      /// <summary>
      /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public bool Remove(K key)
      {
         for (var i = 0; i < list.Count; i++) {
            if (Equals(key, list[i].Key))
            {
               list.RemoveAt(i);
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Gets the value associated with the specified key.
      /// </summary>
      /// <returns>
      /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
      /// </returns>
      /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
      public bool TryGetValue(K key, out V value) {
         foreach (var kvp in list) {
            if (Equals(key, kvp.Key))
            {
               value = kvp.Value;
               return true;
            }
         }
         value = default(V);
         return false;
      }

      /// <summary>
      /// Gets or sets the element with the specified key.
      /// </summary>
      /// <returns>
      /// The element with the specified key.
      /// </returns>
      /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
      public V this[K key] { get { return Get(key); } set { AddOrUpdate(key, value); } }

      /// <summary>
      /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      public ICollection<K> Keys { get { return Util.Generate(list.Count, i => list[i].Key); } }

      /// <summary>
      /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
      /// </returns>
      public ICollection<V> Values { get { return Util.Generate(list.Count, i => list[i].Value); } }
   }
}