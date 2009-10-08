using System;
using System.Diagnostics;
using System.IO;

namespace Poplar.Strategies
{
    [DebuggerStepThrough]
    public abstract class FileSystemStrategy
    {
        private Context context;

        public Context Context
        {
            get
            {
                if (context == null)
                {
                    throw new InvalidOperationException("Context should be set.");
                }
                return context;
            }
            set { context = value; }
        }

        public virtual DirectoryInfo Process(DirectoryInfo source, DirectoryInfo destination)
        {
            return destination;
        }

        public virtual FileInfo Process(FileInfo source, FileInfo destination)
        {
            return destination;
        }
    }
}