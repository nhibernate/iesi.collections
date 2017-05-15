using System.Reflection;

namespace Iesi.Collections.Test
{
	public static class Program
	{
		public static int Main(string[] args)
		{
#if NETCOREAPP1_0
			return new NUnitLite.AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args);
#else
			return new NUnitLite.AutoRun(typeof(Program).Assembly).Execute(args);
#endif
		}
	}
}
