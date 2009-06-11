using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Spark;
using Spark.FileSystem;

namespace Poplar.Strategies
{
    public class TextSubstitutionStrategy : FileSystemStrategy
    {
        // Text substitution is not attempted for files with these extensions
        // TODO: make this configurable
        private readonly List<string> extensions;

        [DebuggerStepThrough]
        public TextSubstitutionStrategy()
        {
            extensions = new List<string> {".exe", ".dll", ".pdb", ".jpg", ".gif", ".png"};
        }

        public override FileInfo Process(FileInfo source, FileInfo destination)
        {
            if (extensions.Contains(destination.Extension))
            {
                Context.Debug.WriteLine("  skipped text substitution for '{0}'", Context.RelativePathFor(destination));
                return destination;
            }

            SubstituteText(source, destination);
            Stub(source, destination);

            return destination;
        }

        private void SubstituteText(FileSystemInfo source, FileSystemInfo destination)
        {
            var oldcontents = File.ReadAllText(source.FullName);
            var contents = oldcontents;

            foreach (var parameter in Context.Generator.Parameters)
            {
                if (contents.Contains(parameter.Stub))
                {
                    contents = contents.Replace(parameter.Stub, parameter.Value);
                    Context.Debug.WriteLine("  replaced  '{0}' with '{1}'", parameter.Stub, parameter.Value);
                }
            }

            if (!oldcontents.Equals(contents))
            {
                File.WriteAllText(destination.FullName, contents);
            }
        }

        private void Stub(FileSystemInfo source, FileSystemInfo destination)
        {
            if (source.FullName.EndsWith(Context.TemplateSuffix))
            {
                // At this time the destination is already copied and processed by
                // 'simple' text substitution, so destination should be processed.
                var settings = new SparkSettings();
                settings.AddNamespace("System");

                var engine = new SparkViewEngine(settings) {DefaultPageBaseType = typeof(Template).FullName};
                var folder = new InMemoryViewFolder {{source.Name, File.ReadAllText(destination.FullName)}};
                engine.ViewFolder = folder;

                var descriptor = new SparkViewDescriptor();
                descriptor.AddTemplate(source.Name);

                var view = (Template) engine.CreateInstance(descriptor);
                var builder = new StringBuilder();
                using (var output = new StringWriter(builder))
                {
                    view.RenderView(output);
                    File.WriteAllText(destination.FullName, output.ToString());
                }

                engine.ReleaseInstance(view);
            }
        }
    }

    public abstract class Template : AbstractSparkView
    {
    }
}