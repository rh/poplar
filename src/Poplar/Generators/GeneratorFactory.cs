using System.Diagnostics;
using System.IO;
using System.Xml;
using Poplar.Parameters;

namespace Poplar.Generators
{
    public class GeneratorFactory
    {
        [DebuggerStepThrough]
        public GeneratorFactory(Context context)
        {
            Context = context;
        }

        public Context Context { get; private set; }

        public Generator Create(string generatorName)
        {
            if (!Directory.Exists(Context.GeneratorDirectory))
            {
                return null;
            }

            var generator = new Generator(Context) {Name = generatorName};

            if (File.Exists(Context.GeneratorConfigFile))
            {
                var document = new XmlDocument();
                document.Load(Context.GeneratorConfigFile);
                generator.Description = document.SelectSingleNode("/generator/description").InnerText;

                foreach (XmlNode node in document.SelectNodes("//parameters/parameter"))
                {
                    var parameter = new Parameter(node);
                    generator.Parameters.Add(parameter);
                }
            }

            if (File.Exists(Context.ParameterConfigFile))
            {
                var document = new XmlDocument();
                document.Load(Context.ParameterConfigFile);

                foreach (XmlNode node in document.SelectNodes("//parameters/parameter"))
                {
                    var name = node.SelectSingleNode("@name").Value;
                    var value = node.SelectSingleNode("@value").Value;
                    var parameter = generator.Parameters[name];
                    if (parameter != null)
                    {
                        parameter.Value = value;
                    }
                }
            }

            return generator;
        }
    }
}