using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NUnit.Framework;
using System.Linq;
#if !NETCOREAPP1_1
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Iesi.Collections.Test.Generic
{
	[TestFixture]
	public class LinkedHashSetFixture : GenericSetFixture
	{
		protected override ISet<string> CreateInstance()
		{
			return new LinkedHashSet<string>();
		}

		protected override ISet<string> CreateInstance(ICollection<string> init)
		{
			return new LinkedHashSet<string>(init);
		}

		protected override Type ExpectedType
		{
			get { return typeof(LinkedHashSet<string>); }
		}


		[Test]
		public void CanIterateInInsertionOrder()
		{
			// Deliberatly add in an order different from the natural ordering.
			var set = new LinkedHashSet<int> { 1, 10, 5 };

			Assert.That(set, Has.Count.EqualTo(3));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 1, 10, 5 }));
		}


		[Test]
		public void ReinsertShouldNotAffectOrdering()
		{
			// Deliberatly add in an order different from the natural ordering.
			var set = new LinkedHashSet<int> { 1, 10, 5 };
			var added = set.Add(1);  // This element should still be first in the list.

			Assert.That(added, Is.False);
			Assert.That(set, Has.Count.EqualTo(3));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 1, 10, 5 }));
		}


		[Test]
		public void ShouldPreserveOrderingOnUnion()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5 };
			set.UnionWith(new int[] { 10, 30, 15 });

			Assert.That(set, Has.Count.EqualTo(5));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 1, 10, 5, 30, 15 }));
		}


		[Test]
		public void ShouldPreserveOrderingOnIntersect()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5, 7, 8, 9 };
			set.IntersectWith(new int[] { 7, 10, 9, 18 });

			Assert.That(set, Has.Count.EqualTo(3));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 10, 7, 9 }));
		}


		[Test]
		public void ShouldPreserveOrderingOnExcept()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5, 7, 8, 9 };
			set.ExceptWith(new int[] { 7, 10, 9, 18 });

			Assert.That(set, Has.Count.EqualTo(3));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 1, 5, 8 }));
		}


		[Test]
		public void ShouldPreserveOrderingOnSymmetricExcept()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5 };
			set.SymmetricExceptWith(new int[] { 1, 10, 3, 9 });

			Assert.That(set, Has.Count.EqualTo(3));
			Assert.That(set.ToArray(), Is.EqualTo(new[] { 5, 3, 9 }));
		}

#if !NETCOREAPP1_1
		[Test(Description = "ES-1")]
		public void DoesNotThrowWhenTryToSerializeWithBinaryFormatter()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5 };

			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				Assert.DoesNotThrow(() =>
					{
						formatter.Serialize(stream, set);
					});
			}
		}

		[Test(Description = "ES-1")]
		public void ShouldBeAbleToDeserializeBinarySerialized()
		{
			var set = new LinkedHashSet<int> { 1, 10, 5 };

			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, set);

				stream.Position = 0;
				var deserialized = (LinkedHashSet<int>) formatter.Deserialize(stream);
				Assert.That(set, Is.EquivalentTo(deserialized));
			}
		}
#endif
	}
}

