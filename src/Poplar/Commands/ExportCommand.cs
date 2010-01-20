using System.IO;
using Optional.Attributes;
using Optional.Commands;
using Poplar.Strategies;

namespace Poplar.Commands
{
	[Description("Exports a generator to a .zip file")]
    [Usage("<generator>")]
	public class ExportCommand : Command, IArgumentsAware
	{
		public override int Execute()
		{
			// 'export' is the first argument
			if (ApplicationContext.Arguments.Length != 2)
			{
				WriteLine("No generator given.");
				return 1;
			}

			Context.Initialize(ApplicationContext.Arguments[1]);
			var generator = Context.Generator;

			if (generator == null)
			{
				WriteLine("Generator '{0}' not found.", Context.GeneratorName);
				return 1;
			}

			var directory = new DirectoryInfo(Context.GeneratorDirectory);

			if (directory.Exists)
			{
				Context.WorkingDirectory = Context.GeneratorsDirectory;

				var path = string.Format("{0}.zip", generator.Name);
				using (var strategy = new ZipStrategy(path) {Context = Context})
				{
					new FileSystemIterator(Context.CurrentDirectory, new[] {strategy}).Iterate(directory);
				}
				Context.Out.WriteLine();
				Context.Out.WriteLine("Created {0}.zip.", generator.Name);
			}
			else
			{
				Context.Out.WriteLine("Generator '{0}' not found.", Context.Generator.Name);
				return 1;
			}

			return 0;
		}
	}
}