/* Copyright © 2002-2004 by Aidant Systems, Inc., and by Jason Smith. */
/* Copyright © 2012 Oskar Berggren */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Iesi.Collections.Generic;

/// <summary>
/// <p>Implements a thread-safe <c>Set</c> wrapper.  The implementation is extremely conservative, 
/// serializing critical sections to prevent possible deadlocks, and locking on everything.
/// The one exception is for enumeration, which is inherently not thread-safe.  For this, you
/// have to <c>lock</c> the <c>SyncRoot</c> object for the duration of the enumeration.</p>
/// </summary>
#if !NETSTANDARD1_0
[Serializable]
#endif
[Obsolete("The SynchronizedSet will be removed in a future version")]
public sealed class SynchronizedSet<T> : ISet<T>
#if !NET40
	, IReadOnlyCollection<T>
#endif
{
	readonly ISet<T> _basisSet;
	readonly object _syncRoot;

	/// <summary>
	/// Constructs a thread-safe <c>ISet</c> wrapper.
	/// </summary>
	/// <param name="basisSet">The <c>Set</c> object that this object will wrap.</param>
	public SynchronizedSet(ISet<T> basisSet)
	{
		_basisSet = basisSet ?? throw new ArgumentNullException(nameof(basisSet));
		_syncRoot = basisSet is ICollection c ? c.SyncRoot : this;
		if (_syncRoot == null)
			throw new ArgumentException("The set you specified returned a null SyncRoot.");
	}

	#region Implementation of ICollection<T>

	/// <summary>
	/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
	void ICollection<T>.Add(T item)
	{
		lock (_syncRoot)
			_basisSet.Add(item);
	}


	/// <summary>
	/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
	public void Clear()
	{
		lock (_syncRoot)
			_basisSet.Clear();
	}

	/// <summary>
	/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
	/// </summary>
	/// <returns>
	/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
	/// </returns>
	/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
	public bool Contains(T item)
	{
		lock (_syncRoot)
			return _basisSet.Contains(item);
	}

	/// <summary>
	/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
	/// </summary>
	/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
	public void CopyTo(T[] array, int arrayIndex)
	{
		lock (_syncRoot)
			_basisSet.CopyTo(array, arrayIndex);
	}

	/// <summary>
	/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <returns>
	/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </returns>
	/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
	public bool Remove(T item)
	{
		lock (_syncRoot)
			return _basisSet.Remove(item);
	}

	/// <summary>
	/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <returns>
	/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </returns>
	public int Count
	{
		get
		{
			lock (_syncRoot)
				return _basisSet.Count;
		}
	}

	/// <summary>
	/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
	/// </summary>
	/// <returns>
	/// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
	/// </returns>
	public bool IsReadOnly
	{
		get
		{
			lock (_syncRoot)
				return _basisSet.IsReadOnly;
		}
	}

	#endregion


	#region Implementation of ISet<T>

	/// <summary>
	/// Modifies the current set so that it contains all elements that are present in either the current set or in the specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public void UnionWith(IEnumerable<T> other)
	{
		lock (_syncRoot)
			_basisSet.UnionWith(other);
	}

	/// <summary>
	/// Modifies the current set so that it contains only elements that are also in a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public void IntersectWith(IEnumerable<T> other)
	{
		lock (_syncRoot)
			_basisSet.IntersectWith(other);
	}

	/// <summary>
	/// Removes all elements in the specified collection from the current set.
	/// </summary>
	/// <param name="other">The collection of items to remove from the set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public void ExceptWith(IEnumerable<T> other)
	{
		lock (_syncRoot)
			_basisSet.ExceptWith(other);
	}

	/// <summary>
	/// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both. 
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		lock (_syncRoot)
			_basisSet.SymmetricExceptWith(other);
	}

	/// <summary>
	/// Determines whether a set is a subset of a specified collection.
	/// </summary>
	/// <returns>
	/// true if the current set is a subset of <paramref name="other"/>; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool IsSubsetOf(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.IsSubsetOf(other);
	}

	/// <summary>
	/// Determines whether the current set is a superset of a specified collection.
	/// </summary>
	/// <returns>
	/// true if the current set is a superset of <paramref name="other"/>; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool IsSupersetOf(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.IsSupersetOf(other);
	}

	/// <summary>
	/// Determines whether the current set is a correct superset of a specified collection.
	/// </summary>
	/// <returns>
	/// true if the <see cref="T:System.Collections.Generic.ISet`1"/> object is a correct superset of <paramref name="other"/>; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set. </param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.IsProperSupersetOf(other);
	}

	/// <summary>
	/// Determines whether the current set is a property (strict) subset of a specified collection.
	/// </summary>
	/// <returns>
	/// true if the current set is a correct subset of <paramref name="other"/>; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.IsProperSubsetOf(other);
	}

	/// <summary>
	/// Determines whether the current set overlaps with the specified collection.
	/// </summary>
	/// <returns>
	/// true if the current set and <paramref name="other"/> share at least one common element; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool Overlaps(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.Overlaps(other);
	}

	/// <summary>
	/// Determines whether the current set and the specified collection contain the same elements.
	/// </summary>
	/// <returns>
	/// true if the current set is equal to <paramref name="other"/>; otherwise, false.
	/// </returns>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	public bool SetEquals(IEnumerable<T> other)
	{
		lock (_syncRoot)
			return _basisSet.SetEquals(other);
	}


	/// <summary>
	/// Adds an element to the current set and returns a value to indicate if the element was successfully added. 
	/// </summary>
	/// <returns>
	/// true if the element is added to the set; false if the element is already in the set.
	/// </returns>
	/// <param name="item">The element to add to the set.</param>
	public bool Add(T item)
	{
		lock (_syncRoot)
			return _basisSet.Add(item);
	}

	#endregion


	#region Implementation of IEnumerable

	/// <summary>
	/// Returns an enumerator that iterates through a collection. Enumeration is inherently not
	/// thread-safe. Use a <c>lock</c> on the <c>SyncRoot</c> to synchronize the entire enumeration process.
	/// </summary>
	/// <returns>
	/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
	/// </returns>
	/// <filterpriority>2</filterpriority>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion


	#region Implementation of IEnumerable<out T>

	/// <summary>
	/// Returns an enumerator that iterates through the collection. Enumeration is inherently not
	/// thread-safe. Use a <c>lock</c> on the <c>SyncRoot</c> to synchronize the entire enumeration process.
	/// </summary>
	/// <returns>
	/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
	/// </returns>
	/// <filterpriority>1</filterpriority>
	public IEnumerator<T> GetEnumerator()
	{
		lock (_syncRoot)
			return _basisSet.GetEnumerator();
	}

	#endregion

}
