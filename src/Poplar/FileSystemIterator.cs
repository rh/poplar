using System.Collections.Generic;
using System.IO;
using Poplar.Strategies;

namespace Poplar
{
    public class FileSystemIterator
    {
        // ignores contains names of files which contain generator metadata
        // TODO: make this configurable
        private readonly List<string> ignores;
        private readonly FileSystemStrategy[] strategies;

        public FileSystemIterator(string workingDirectory, FileSystemStrategy[] strategies)
        {
            ignores = new List<string> {"manifest.xml"};
            WorkingDirectory = workingDirectory;
            this.strategies = strategies;
        }

        protected string WorkingDirectory { get; private set; }

        public void Iterate(DirectoryInfo source)
        {
            foreach (var directory in source.GetDirectories())
            {
                IterateInternal(directory, new DirectoryInfo(Path.Combine(WorkingDirectory, directory.Name)));
            }

            foreach (var file in source.GetFiles())
            {
                IterateInternal(file, new FileInfo(Path.Combine(WorkingDirectory, file.Name)));
            }
        }

        private void IterateInternal(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var strategy in strategies)
            {
                destination = strategy.Process(source, destination);
            }

            foreach (var directory in source.GetDirectories())
            {
                IterateInternal(directory, new DirectoryInfo(Path.Combine(destination.FullName, directory.Name)));
            }

            foreach (var file in source.GetFiles())
            {
                IterateInternal(file, new FileInfo(Path.Combine(destination.FullName, file.Name)));
            }
        }

        private void IterateInternal(FileInfo source, FileInfo destination)
        {
            if (ignores.Contains(source.Name))
            {
                return;
            }

            foreach (var strategy in strategies)
            {
                destination = strategy.Process(source, destination);
            }
        }
    }
}