using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Iesi.Collections.Test.Generic
{
	/// <summary>
	/// Summary description for SetFixture.
	/// </summary>
	public abstract class GenericSetFixture
	{
		private IList<string> _aInitValues;
		private IList<string> _bInitValues;
		protected ISet<string> _set;

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
				Assert.IsTrue(_set.Contains(obj), "set should contain the object in the array");
				count++;
			}

			Assert.AreEqual(3, count, "should have 3 items in array");
		}

		[Test]
		public void Count()
		{
			Assert.AreEqual(3, _set.Count, "should be 3 items");
			Assert.AreEqual(0, CreateInstance().Count, "new set should have nothing in it.");
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

			Assert.AreEqual(3, init.Count, "3 items in set");

			int index = 0;
			foreach (string obj in init)
			{
				Assert.IsTrue(theSet.Contains(obj), "set should contain obj at index = " + index.ToString());
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
				Assert.IsTrue(_set.Add("four"), "should have added 'four'");
				Assert.AreEqual(4, _set.Count, "should have added 'four'");

				Assert.IsFalse(_set.Add(two), "'two' was already there");
				Assert.AreEqual(4, _set.Count, "object already in set");
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
				Assert.AreEqual(5, _set.Count, "should have added one 'four' and 'five'");

				_set.UnionWith(addAll);
				Assert.AreEqual(5, _set.Count, "no new objects should have been added");

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
				Assert.AreEqual(0, _set.Count, "should have no items in ISet.");

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
			Assert.IsTrue(_set.Contains(one), "does contain one");
			Assert.IsFalse(_set.Contains("four"), "does not contain 'four'");
		}

		[Test]
		public void IsSupersetOf()
		{
			List<string> all = new List<string>(2);
			all.Add("one");
			all.Add("two");

			Assert.IsTrue(_set.IsSupersetOf(all), "should contain 'one' and 'two'");

			all.Add("not in there");
			Assert.IsFalse(_set.IsSupersetOf(all), "should not contain the just added 'not in there'");
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

				Assert.AreEqual(3, a.Count, "contains 3 elements - 'zero', 'one', and 'four'");
				Assert.IsTrue(a.Contains("zero"), "should contain 'zero'");
				Assert.IsTrue(a.Contains("one"), "should contain 'one'");
				Assert.IsTrue(a.Contains("four"), "should contain 'four'");

				Assert.IsTrue(b.IsSupersetOf(_bInitValues), "should not have modified b");
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

				Assert.AreEqual(2, a.Count, "contains 2 elements - 'two', and 'three'");
				Assert.IsTrue(a.Contains("two"), "should contain 'two'");
				Assert.IsTrue(a.Contains("three"), "should contain 'three'");

				Assert.IsTrue(b.IsSupersetOf(_bInitValues), "should not have modified b");
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

				Assert.AreEqual(2, a.Count, "contains 2 elements - 'zero', and 'one'");
				Assert.IsTrue(a.Contains("zero"), "should contain 'zero'");
				Assert.IsTrue(a.Contains("one"), "should contain 'one'");

				Assert.IsTrue(b.IsSupersetOf(_bInitValues), "should not have modified b");
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
				Assert.IsTrue(_set.Remove(one), "should have removed 'one'");
				Assert.IsFalse(_set.Contains(one), "one should have been removed");
				Assert.AreEqual(2, _set.Count, "should be 2 items after one removed.");

				Assert.IsFalse(_set.Remove(one), "was already removed.");
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
}