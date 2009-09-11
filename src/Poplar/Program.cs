using System;
using Optional.Commands;
using Poplar.Commands;
using Command=Poplar.Commands.Command;

namespace Poplar
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var context = new Context();

                HelpCommand.DisplayCopyRight = HelpCommand.DisplayDescription = false;

                var factory = new CommandFactory(args);
                factory.Register<ImportCommand>();
                factory.Register<RemoveCommand>();

                foreach (var generator in context.Generators)
                {
                    factory.Register(new GenerateCommand {Name = generator.Name});
                }

                var command = factory.Create(args);
                if (command is Command)
                {
                    (command as Command).GeneratorContext = context;
                }
                command.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}