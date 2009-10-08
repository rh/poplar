using System;
using System.Diagnostics;

namespace Poplar.Commands
{
    [DebuggerStepThrough]
    public class GenerateCommand : Command
    {
        public override int Execute()
        {
            GeneratorContext.Initialize(Name);
            var generator = GeneratorContext.Generator;

            if (generator == null)
            {
                WriteLine("Generator '{0}' not found.", GeneratorContext.GeneratorName);
                return 1;
            }

            foreach (var parameter in generator.Parameters)
            {
                if (!parameter.HasValue)
                {
                    Console.Write("{0}: ", parameter.Name);
                    parameter.Value = Console.ReadLine();
                }
            }

            generator.Execute();
            return 0;
        }
    }
}