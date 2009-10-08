using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Poplar.Generators;

namespace Poplar
{
    public class Context
    {
        protected const string GeneratorDirectoryName = "generators";

        /// <summary>
        /// Gets or sets the output stream.
        /// </summary>
        public TextWriter Out { get; set; }

        //[DebuggerStepThrough]
        public Context()
        {
            Out = Console.Out;
            GeneratorName = String.Empty;
            WorkingDirectory = Directory.GetCurrentDirectory();

            Generators = new List<Generator>();

            var factory = new GeneratorFactory(this);
            var generatorsDirectory = new DirectoryInfo(GeneratorsDirectory);

            if (!Directory.Exists(generatorsDirectory.FullName))
            {
                Directory.CreateDirectory(generatorsDirectory.FullName);
            }

            foreach (var directory in generatorsDirectory.GetDirectories())
            {
                var generator = factory.Create(directory.Name);
                Generators.Add(generator);
            }
        }

        #region Generator Specific

        public void Initialize(string name)
        {
            GeneratorName = name;
            var factory = new GeneratorFactory(this);
            Generator = factory.Create(GeneratorName);
        }

        public string GeneratorName { get; set; }

        public Generator Generator { get; private set; }

        public bool GeneratorExists
        {
            get { return Directory.Exists(GeneratorDirectory); }
        }

        /// <summary>
        /// Gets the directory which contains all generators.
        /// </summary>
        public string GeneratorsDirectory
        {
            [DebuggerStepThrough]
            get { return Path.Combine(ApplicationDataDirectory, GeneratorDirectoryName); }
        }

        /// <summary>
        /// Gets the directory which contains the current generator.
        /// </summary>
        public string GeneratorDirectory
        {
            [DebuggerStepThrough]
            get { return Path.Combine(GeneratorsDirectory, GeneratorName); }
        }

        public IList<Generator> Generators { get; protected set; }

        public string GeneratorConfigFile
        {
            [DebuggerStepThrough]
            get { return Path.Combine(GeneratorsDirectory, GeneratorName + ".xml"); }
        }

        public string ParameterConfigFile
        {
            [DebuggerStepThrough]
            get { return Path.Combine(CurrentDirectory, "poplar.xml"); }
        }

        #endregion

        #region Environment Specific

        /// <summary>
        /// Gets the directory in which this assembly is located.
        /// </summary>
        public string AssemblyDirectory
        {
            [DebuggerStepThrough]
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        public string ApplicationDataDirectory
        {
            [DebuggerStepThrough]
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Poplar"); }
        }

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        public string CurrentDirectory
        {
            [DebuggerStepThrough]
            get { return Directory.GetCurrentDirectory(); }
        }

        [DebuggerStepThrough]
        public string RelativePathFor(FileSystemInfo destination)
        {
            return destination.FullName.Substring(WorkingDirectory.Length + 1);
        }

        public string WorkingDirectory { get; set; }

        #endregion
    }
}