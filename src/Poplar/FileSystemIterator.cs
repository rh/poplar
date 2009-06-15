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
        private readonly string workingDirectory;
        private readonly FileSystemStrategy[] strategies;

        public FileSystemIterator(string workingDirectory, FileSystemStrategy[] strategies)
        {
            ignores = new List<string> {"manifest.xml"};
            this.workingDirectory = workingDirectory;
            this.strategies = strategies;
        }

        public string WorkingDirectory
        {
            get { return workingDirectory; }
        }

        public void Iterate(DirectoryInfo source)
        {
            foreach (var directory in source.GetDirectories())
            {
                IterateInternal(directory, Path.Combine(WorkingDirectory, directory.Name));
            }

            foreach (var file in source.GetFiles())
            {
                IterateInternal(file, Path.Combine(WorkingDirectory, file.Name));
            }
        }

        private void IterateInternal(DirectoryInfo source, string destination)
        {
            IterateInternal(source, new DirectoryInfo(destination));
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

        private void IterateInternal(FileInfo source, string destination)
        {
            IterateInternal(source, new FileInfo(destination));
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