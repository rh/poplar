using System.IO;
using Optional.Attributes;
using Optional.Commands;
using Poplar.Strategies;

namespace Poplar.Commands
{
    [Description("Imports an entire directory as a generator")]
    [Usage("<directory> [options]")]
    public class ImportCommand : Command, IArgumentsAware
    {
        [ShortName("n"), LongName("name")]
        [Description("The name of the new generator.")]
        public string Name { get; set; }

        public override int Execute()
        {
            if (ApplicationContext.Options.Count < 2)
            {
                WriteLine("Please specify a directory to import.");
                return 1;
            }

            var directory = ApplicationContext.Arguments[1];

            // Create the generator directory
            // TODO: this should also work with a proper path, e.g. C:\temp
            var source = new DirectoryInfo(Path.Combine(Context.CurrentDirectory, directory));
            var destination = new DirectoryInfo(Path.Combine(Context.GeneratorsDirectory, source.Name));

            foreach (var option in ApplicationContext.Options)
            {
                if (option.ShortName == "n" || option.LongName == "name")
                {
                    Name = option.Value;
                    break;
                }
            }

            if (Name != null)
            {
                destination = new DirectoryInfo(Path.Combine(Context.GeneratorsDirectory, Name));
            }

            // TODO: check if a generator with that name already exists
            Context.WorkingDirectory = Context.GeneratorsDirectory;

            // Use CopyStrategy so the output is consistent
            var strategy = new CopyStrategy {Context = Context};
            strategy.Process(source, destination);

            // Copy all directories and files from the source directory
            var strategies = new FileSystemStrategy[] {strategy};
            // Use destination.FullName so the output contains the name of the new generator
            new FileSystemIterator(destination.FullName, strategies).Iterate(source);

            return 0;
        }
    }
}