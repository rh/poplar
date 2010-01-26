using System;

// ReSharper disable InconsistentNaming

namespace Poplar.Dsl
{
	public abstract class PoplarTemplate
	{
		public abstract void Execute();

		public void template(string name, Action action)
		{
			Console.WriteLine("Template '{0}' created...", name);
			action();
		}

		public void description(string value)
		{
			Console.WriteLine("Description: {0}", value);
		}

		public void parameter(string name, Action action)
		{
			Console.WriteLine("Adding parameter '{0}'...", name);
			action();
		}
	}
}

// ReSharper restore InconsistentNaming