using System;
using System.Diagnostics;
using System.IO;
using Poplar.Parameters;
using Poplar.Strategies;

namespace Poplar.Generators
{
	public class Generator
	{
		public Context Context { get; private set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public ParameterCollection Parameters { get; set; }

		[DebuggerStepThrough]
		public Generator(Context context)
		{
			Context = context;
			Name = String.Empty;
			Description = String.Empty;
			Parameters = new ParameterCollection();
		}

		public void Execute()
		{
			var directory = new DirectoryInfo(Context.GeneratorDirectory);

			if (directory.Exists)
			{
				var strategies = new FileSystemStrategy[]
					{
						new RenamingStrategy(),
						new CopyStrategy(),
						new TextSubstitutionStrategy()
					};
				foreach (var strategy in strategies)
				{
					strategy.Context = Context;
				}

				new FileSystemIterator(Context.CurrentDirectory, strategies).Iterate(directory);
			}
			else
			{
				Context.Out.WriteLine("Generator '{0}' not found.", Context.Generator.Name);
			}
		}

		[DebuggerStepThrough]
		public override string ToString()
		{
			return Name;
		}
	}
}