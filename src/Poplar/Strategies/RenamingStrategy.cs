using System.IO;

namespace Poplar.Strategies
{
    public class RenamingStrategy : FileSystemStrategy
    {
        public override DirectoryInfo Process(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var parameter in Context.Generator.Parameters)
            {
                if (source.Name.Contains(parameter.Stub))
                {
                    var path = Path.Combine(destination.Parent.FullName,
                                            source.Name.Replace(parameter.Stub, parameter.Value));
                    destination = new DirectoryInfo(path);
                    Context.Debug.WriteLine("  renamed   '{0}' to '{1}'", source.Name, destination.Name);
                }
            }
            return destination;
        }

        public override FileInfo Process(FileInfo source, FileInfo destination)
        {
            foreach (var parameter in Context.Generator.Parameters)
            {
                if (source.Name.Contains(parameter.Stub))
                {
                    var path = Path.Combine(destination.Directory.FullName,
                                            source.Name.Replace(parameter.Stub, parameter.Value));
                    destination = new FileInfo(path);
                    Context.Debug.WriteLine("  renamed   '{0}' to '{1}'", source.Name, destination.Name);
                }
            }
            if (source.Name.EndsWith(Context.TemplateSuffix))
            {
                var path = Path.Combine(destination.Directory.FullName, source.Name.Substring(0, source.Name.Length - Context.TemplateSuffix.Length));
                destination = new FileInfo(path);
                Context.Debug.WriteLine("  renamed   '{0}' to '{1}'", source.Name, destination.Name);
            }
            return destination;
        }
    }
}