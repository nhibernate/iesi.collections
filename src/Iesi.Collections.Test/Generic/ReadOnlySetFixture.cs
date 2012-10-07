using System;
using System.Collections.Generic;
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
	}
}