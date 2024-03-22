using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Iesi.Collections.Test.Generic;

/// <summary>
/// Summary description for SetFixture.
/// </summary>
public abstract class GenericSetFixture
{
	IList<string> _aInitValues = default!;
	IList<string> _bInitValues = default!;
	protected ISet<string> _set = default!;

	public static string one = "one";
	public static string two = "two";
	public static string three = "three";

	[SetUp]
	public virtual void SetUp()
	{
		_aInitValues = new List<string>();
		_aInitValues.Add("zero");
		_aInitValues.Add("one");
		_aInitValues.Add("two");
		_aInitValues.Add("three");

		_bInitValues = new List<string>();
		_bInitValues.Add("two");
		_bInitValues.Add("three");
		_bInitValues.Add("four");

		_set = CreateInstance(new string[] { one, two, three });
	}


	#region System.Collections.ICollection Member Tests

	[Test]
	public void CopyTo()
	{
		string[] dest = new string[3];
		_set.CopyTo(dest, 0);

		int count = 0;

		foreach (string obj in dest)
		{
			Assert.That(_set.Contains(obj), "set should contain the object in the array");
			count++;
		}

		Assert.That(count, Is.EqualTo(3), "should have 3 items in array");
	}

	[Test]
	public void Count()
	{
		Assert.That(_set, Has.Count.EqualTo(3), "should be 3 items");
		Assert.That(CreateInstance(), Is.Empty, "new set should have nothing in it.");
	}

	#endregion

	#region Iesi.Collections.ISet<string> Constructor Tests

	[Test]
	public void CtorWithDefaults()
	{
		List<string> init = new List<string>(3);
		init.Add("one");
		init.Add("two");
		init.Add("three");

		ISet<string> theSet = CreateInstance(init);

		Assert.That(init, Has.Count.EqualTo(3), "3 items in set");

		int index = 0;
		foreach (string obj in init)
		{
			Assert.That(theSet.Contains(obj), "set should contain obj at index = " + index.ToString());
			index++;
		}
	}

	#endregion

	#region Iesi.Collections.ISet<string> Member Tests

	[Test]
	public void Add()
	{
		try
		{
			Assert.That(_set.Add("four"), "should have added 'four'");
			Assert.That(_set, Has.Count.EqualTo(4), "should have added 'four'");

			Assert.That(_set.Add(two), Is.False, "'two' was already there");
			Assert.That(_set, Has.Count.EqualTo(4), "object already in set");
			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	[Test]
	public void UnionWith()
	{
		List<string> addAll = new List<string>(3);
		addAll.Add("four");
		addAll.Add("five");
		addAll.Add("four");

		try
		{
			_set.UnionWith(addAll);
			Assert.That(_set, Has.Count.EqualTo(5), "should have added one 'four' and 'five'");

			_set.UnionWith(addAll);
			Assert.That(_set, Has.Count.EqualTo(5), "no new objects should have been added");

			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	[Test]
	public void Clear()
	{
		try
		{
			_set.Clear();
			Assert.That(_set, Is.Empty, "should have no items in ISet.");

			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	[Test]
	public void Contains()
	{
		Assert.That(_set.Contains(one), "does contain one");
		Assert.That(_set.Contains("four"), Is.False, "does not contain 'four'");
	}

	[Test]
	public void IsSupersetOf()
	{
		List<string> all = new List<string>(2);
		all.Add("one");
		all.Add("two");

		Assert.That(_set.IsSupersetOf(all), "should contain 'one' and 'two'");

		all.Add("not in there");
		Assert.That(_set.IsSupersetOf(all), Is.False, "should not contain the just added 'not in there'");
	}


	[Test]
	public void IsSubsetOfTest()
	{
		Assert.That(_set.IsSubsetOf(new[] { "one", "two" }), Is.False);
		Assert.That(_set.IsSubsetOf(new[] { "one", "two", "three" }));
		Assert.That(_set.IsSubsetOf(new[] { "one", "two", "three", "four" }));
	}


	[Test]
	public void IsProperSubsetOfTest()
	{
		Assert.That(_set.IsProperSubsetOf(new[] { "one", "two", "three" }), Is.False);
		Assert.That(_set.IsProperSubsetOf(new[] { "one", "two", "nine" }), Is.False);
		Assert.That(_set.IsProperSubsetOf(new[] { "one", "two", "three", "nine" }));
	}


	[Test]
	public void IsProperSupersetOfTest()
	{
		Assert.That(_set.IsProperSupersetOf(new[] { "one", "two", "three" }), Is.False);
		Assert.That(_set.IsProperSupersetOf(new[] { "one", "two", "nine" }), Is.False);
		Assert.That(_set.IsProperSupersetOf(new[] { "one", "two" }));
	}


	[Test]
	public void OverlapsTest()
	{
		Assert.That(_set.Overlaps(new[] { "one", "two", "three" }));
		Assert.That(_set.Overlaps(new[] { "one", "two" }));
		Assert.That(_set.Overlaps(new[] { "six", "seven" }), Is.False);
	}


	[Test]
	public void SetEqualsTest()
	{
		Assert.That(_set.SetEquals(new[] { "one", "two", "three" }));
		Assert.That(_set.SetEquals(new[] { "one", "two", "three", "four" }), Is.False);
		Assert.That(_set.SetEquals(new[] { "one", "two" }), Is.False);
	}


	[Test]
	public void SymmetricExceptWith()
	{
		try
		{
			ISet<string> a = CreateInstance(_aInitValues);
			ISet<string> b = CreateInstance(_bInitValues);

			a.SymmetricExceptWith(b);
			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");

			Assert.That(a, Has.Count.EqualTo(3), "contains 3 elements - 'zero', 'one', and 'four'");
			Assert.That(a.Contains("zero"), "should contain 'zero'");
			Assert.That(a.Contains("one"), "should contain 'one'");
			Assert.That(a.Contains("four"), "should contain 'four'");

			Assert.That(b.IsSupersetOf(_bInitValues), "should not have modified b");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	[Test]
	public void IntersectWith()
	{
		try
		{
			ISet<string> a = CreateInstance(_aInitValues);
			ISet<string> b = CreateInstance(_bInitValues);

			a.IntersectWith(b);
			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");

			Assert.That(a, Has.Count.EqualTo(2), "contains 2 elements - 'two', and 'three'");
			Assert.That(a.Contains("two"), "should contain 'two'");
			Assert.That(a.Contains("three"), "should contain 'three'");

			Assert.That(b.IsSupersetOf(_bInitValues), "should not have modified b");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}


	[Test]
	public void ExceptWith()
	{
		try
		{
			ISet<string> a = CreateInstance(_aInitValues);
			ISet<string> b = CreateInstance(_bInitValues);

			a.ExceptWith(b);
			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");

			Assert.That(a, Has.Count.EqualTo(2), "contains 2 elements - 'zero', and 'one'");
			Assert.That(a.Contains("zero"), "should contain 'zero'");
			Assert.That(a.Contains("one"), "should contain 'one'");

			Assert.That(b.IsSupersetOf(_bInitValues), "should not have modified b");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	[Test]
	public void Remove()
	{
		try
		{
			Assert.That(_set.Remove(one), "should have removed 'one'");
			Assert.That(_set.Contains(one), Is.False, "one should have been removed");
			Assert.That(_set, Has.Count.EqualTo(2), "should be 2 items after one removed.");

			Assert.That(_set.Remove(one), Is.False, "was already removed.");
			if (_set.IsReadOnly)
				Assert.Fail("Read-only set can be modified");
		}
		catch (NotSupportedException)
		{
			if (!_set.IsReadOnly)
				throw;
		}
	}

	#endregion

	protected abstract ISet<string> CreateInstance();

	protected abstract ISet<string> CreateInstance(ICollection<string> init);

	protected abstract Type ExpectedType { get; }
}
