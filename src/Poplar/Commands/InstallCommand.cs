using System;
using Optional.Attributes;
using Optional.Commands;

namespace Poplar.Commands
{
    [Usage("<generator>")]
    public class InstallCommand : Command, IArgumentsAware
    {
        public override int Execute()
        {
            // 'install' is the first argument
            if (ApplicationContext.Arguments.Length != 2)
            {
                WriteLine("No generator given.");
                return 1;
            }

            Context.Initialize(ApplicationContext.Arguments[1]);
            var generator = Context.Generator;

            if (generator == null)
            {
                WriteLine("Generator '{0}' not found.", Context.GeneratorName);
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