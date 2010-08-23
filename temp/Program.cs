using System;
using System.Globalization;
using System.Threading;
using Poplar.Dsl;
using Rhino.DSL;

namespace Poplar
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(1033);

            try
            {
                var factory = new DslFactory();
                factory.Register<PoplarTemplateFactory>(new PoplarDslEngine());
                var templateFactory = factory.Create<PoplarTemplateFactory>(args[0], null);
                templateFactory.Initialize();

                var template = templateFactory.Template;
                Console.WriteLine("{0}: {1}", template.Name, template.Description);
                foreach (var parameter in template.Parameters)
                {
                    Console.WriteLine("  {0}", parameter);
                    Console.WriteLine("    value: {0}", parameter.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}