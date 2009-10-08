using System.Diagnostics;
using System.IO;

namespace Poplar.Strategies
{
    [DebuggerStepThrough]
    public class CopyStrategy : FileSystemStrategy
    {
        private const bool OverwriteExistingFile = true;

        public override DirectoryInfo Process(DirectoryInfo source, DirectoryInfo destination)
        {
            Context.Out.WriteLine("  {0}    {1}", destination.Exists ? "exists" : "create", Context.RelativePathFor(destination));
            if (!destination.Exists)
            {
                destination.Create();
            }
            return destination;
        }

        public override FileInfo Process(FileInfo source, FileInfo destination)
        {
            Context.Out.WriteLine("  {0}   {1}", destination.Exists ? "replace" : "create ", Context.RelativePathFor(destination));
            source.CopyTo(destination.FullName, OverwriteExistingFile);
            return destination;
        }
    }
}