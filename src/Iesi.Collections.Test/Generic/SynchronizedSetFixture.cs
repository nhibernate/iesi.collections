using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NUnit.Framework;
#if !NETCOREAPP1_0
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Iesi.Collections.Test.Generic;

/// <summary>
/// Summary description for ReadOnlySetFixture.
/// </summary>
[TestFixture]
[Obsolete("SynchronizedSet is obsolete")]
public class SynchronizedSetFixture : GenericSetFixture
{
	protected override ISet<string> CreateInstance()
	{
		return new SynchronizedSet<string>(new HashSet<string>());
	}

	protected override ISet<string> CreateInstance(ICollection<string> init)
	{
		return new SynchronizedSet<string>(new HashSet<string>(init));
	}

	protected override Type ExpectedType => typeof(SynchronizedSet<string>);

#if !NETCOREAPP1_0
	[Test(Description = "ES-1")]
	[Obsolete("BinaryFormatter is obsolete")]
	public void ShouldBeAbleToDeserializeBinarySerialized()
	{
		var set = new SynchronizedSet<int>(new HashSet<int> { 1, 10, 5 });

		var formatter = new BinaryFormatter();
		using (var stream = new MemoryStream())
		{
			formatter.Serialize(stream, set);

			stream.Position = 0;
			var deserialized = (SynchronizedSet<int>)formatter.Deserialize(stream);
			Assert.That(set, Is.EquivalentTo(deserialized));
		}
	}
#endif

}
