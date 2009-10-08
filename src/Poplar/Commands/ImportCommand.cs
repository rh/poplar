using System.IO;
using Optional.Attributes;
using Poplar.Strategies;

namespace Poplar.Commands
{
    [Description("Imports an entire directory as a generator")]
    public class ImportCommand : Command
    {
        [Required, Description("The directory to import.")]
        public string Directory { get; set; }

        [Description("The name of the new generator.")]
        public string Generator { get; set; }

        public override int Execute()
        {
            // Create the generator directory
            var source = new DirectoryInfo(Path.Combine(GeneratorContext.CurrentDirectory, Directory));
            var destination = new DirectoryInfo(Path.Combine(GeneratorContext.GeneratorsDirectory, source.Name));

            if (Generator != null)
            {
                destination = new DirectoryInfo(Path.Combine(GeneratorContext.GeneratorsDirectory, Generator));
            }

            GeneratorContext.WorkingDirectory = GeneratorContext.GeneratorsDirectory;

            // Use CopyStrategy so the output is consistent
            var strategy = new CopyStrategy {Context = GeneratorContext};
            strategy.Process(source, destination);

            // Copy all directories and files from the source directory
            var strategies = new FileSystemStrategy[] {strategy};
            // Use destination.FullName so the output contains the name of the new generator
            new FileSystemIterator(destination.FullName, strategies).Iterate(source);

            return 0;
        }
    }
}