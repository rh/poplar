using System.IO;
using Optional.Attributes;
using Optional.Commands;
using Poplar.Strategies;

namespace Poplar.Commands
{
    [Description("Exports a generator to a .zip file")]
    public class ExportCommand : Command, IArgumentsAware
    {
        public override int Execute()
        {
            // 'export' is the first argument
            if (ApplicationContext.ApplicationArguments.Length != 2)
            {
                WriteLine("No generator given.");
                return 1;
            }

            GeneratorContext.Initialize(ApplicationContext.ApplicationArguments[1]);
            var generator = GeneratorContext.Generator;

            if (generator == null)
            {
                WriteLine("Generator '{0}' not found.", GeneratorContext.GeneratorName);
                return 1;
            }

            var directory = new DirectoryInfo(GeneratorContext.GeneratorDirectory);

            if (directory.Exists)
            {
                GeneratorContext.WorkingDirectory = GeneratorContext.GeneratorsDirectory;

                var path = string.Format("{0}.zip", generator.Name);
                using (var strategy = new ZipStrategy(path) {Context = GeneratorContext})
                {
                    new FileSystemIterator(GeneratorContext.CurrentDirectory, new[] {strategy}).Iterate(directory);
                }
                GeneratorContext.Out.WriteLine();
                GeneratorContext.Out.WriteLine("Created {0}.zip.", generator.Name);
            }
            else
            {
                GeneratorContext.Out.WriteLine("Generator '{0}' not found.", GeneratorContext.Generator.Name);
                return 1;
            }

            return 0;
        }
    }
}