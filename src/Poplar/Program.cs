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

				var factory = new CommandFactory();

				factory.Register<ListCommand>();
				factory.Register<InstallCommand>();
				factory.Register<ImportCommand>();
				factory.Register<ExportCommand>();
				factory.Register<RemoveCommand>();

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