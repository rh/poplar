using System;
using Poplar.Dsl;
using Rhino.DSL;

namespace Poplar
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				var factory = new DslFactory();
				factory.Register<PoplarTemplate>(new PoplarDslEngine());
				var template = factory.Create<PoplarTemplate>(args[0], null);
				template.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}