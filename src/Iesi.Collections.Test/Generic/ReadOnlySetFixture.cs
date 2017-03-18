using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace Iesi.Collections.Test.Generic
{
	/// <summary>
	/// Summary description for ReadOnlySetFixture.
	/// </summary>
	[TestFixture]
	public class ReadOnlySetFixture : GenericSetFixture
	{
		protected override ISet<string> CreateInstance()
		{
			return new ReadOnlySet<string>(new HashSet<string>());
		}

		protected override ISet<string> CreateInstance(ICollection<string> init)
		{
			return new ReadOnlySet<string>(new HashSet<string>(init));
		}

		protected override Type ExpectedType
		{
			get { return typeof(ReadOnlySet<string>); }
		}

        [Test(Description = "ES-1")]
        public void ShouldBeAbleToDeserializeBinarySerialized()
        {
            var set = new ReadOnlySet<int>(new HashSet<int> { 1, 10, 5 });

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, set);

                stream.Position = 0;
                var deserialized = (ReadOnlySet<int>)formatter.Deserialize(stream);
                Assert.That(set, Is.EquivalentTo(deserialized));
            }
        }

	}
}