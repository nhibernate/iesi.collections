/* Copyright © 2002-2004 by Aidant Systems, Inc., and by Jason Smith. */
/* Copyright © 2012 Oskar Berggren */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Iesi.Collections.Generic;

/// <summary>
/// <p>Implements a read-only <c>Set</c> wrapper.</p> 
/// <p>Although this is advertised as immutable, it really isn't.  Anyone with access to the
/// wrapped set can still change the set.</p>
/// </summary>
#if !NETSTANDARD1_0
[Serializable]
#endif
public sealed class ReadOnlySet<T> : ISet<T>
#if !NET40
	, IReadOnlyCollection<T>
#endif
{
	const string ErrorMessage = "The set cannot be modified through this instance.";
	readonly ISet<T> _basisSet;


	/// <summary>
	/// Constructs an immutable (read-only) <c>Set</c> wrapper.
	/// </summary>
	/// <param name="basisSet">The <c>Set</c> that is wrapped.</param>
	public ReadOnlySet(ISet<T> basisSet)
	{
		_basisSet = basisSet ?? throw new ArgumentNullException(nameof(basisSet));
	}


	#region Implementation of IEnumerable

	/// <summary>
	/// Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>
	/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
	/// </returns>
	/// <filterpriority>2</filterpriority>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>
	/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
	/// </returns>
	/// <filterpriority>1</filterpriority>
	public IEnumerator<T> GetEnumerator()
	{
		return _basisSet.GetEnumerator();
	}

	#endregion


	#region Implementation of ICollection<T>

	/// <summary>
	/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException(ErrorMessage);
	}


	/// <summary>
	/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ICollection<T>.Clear()
	{
		throw new NotSupportedException(ErrorMessage);
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
		return _basisSet.Contains(item);
	}

	/// <summary>
	/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
	/// </summary>
	/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
	public void CopyTo(T[] array, int arrayIndex)
	{
		_basisSet.CopyTo(array, arrayIndex);
	}

	/// <summary>
	/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <returns>
	/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </returns>
	/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException(ErrorMessage);
	}

	/// <summary>
	/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </summary>
	/// <returns>
	/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	/// </returns>
	public int Count => _basisSet.Count;

	/// <summary>
	/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
	/// </summary>
	/// <returns>
	/// True.
	/// </returns>
	public bool IsReadOnly => true;

	#endregion


	#region Implementation of ISet<T>


	/// <summary>
	/// Adds an element to the current set and returns a value to indicate if the element was successfully added. 
	/// </summary>
	/// <returns>
	/// true if the element is added to the set; false if the element is already in the set.
	/// </returns>
	/// <param name="item">The element to add to the set.</param>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	bool ISet<T>.Add(T item)
	{
		throw new NotSupportedException(ErrorMessage);
	}

	/// <summary>
	/// Modifies the current set so that it contains all elements that are present in both the current set and in the specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ISet<T>.UnionWith(IEnumerable<T> other)
	{
		throw new NotSupportedException(ErrorMessage);
	}

	/// <summary>
	/// Modifies the current set so that it contains only elements that are also in a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ISet<T>.IntersectWith(IEnumerable<T> other)
	{
		throw new NotSupportedException(ErrorMessage);
	}

	/// <summary>
	/// Removes all elements in the specified collection from the current set.
	/// </summary>
	/// <param name="other">The collection of items to remove from the set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ISet<T>.ExceptWith(IEnumerable<T> other)
	{
		throw new NotSupportedException(ErrorMessage);
	}

	/// <summary>
	/// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both. 
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
	/// <exception cref="NotSupportedException"> is always thrown</exception>
	void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
	{
		throw new NotSupportedException(ErrorMessage);
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
		return _basisSet.SetEquals(other);
	}

	#endregion
}
