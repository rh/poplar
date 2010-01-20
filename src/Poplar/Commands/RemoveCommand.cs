using System;
using System.IO;
using System.Security;
using Optional.Attributes;
using Optional.Commands;

namespace Poplar.Commands
{
	[Description("Removes generators")]
    [Usage("<generator>")]
	public class RemoveCommand : Command, IArgumentsAware
	{
		public override int Execute()
		{
			// 'remove' is the first argument
			if (ApplicationContext.Arguments.Length != 2)
			{
				WriteLine("No generator given.");
				return 1;
			}

			Context.GeneratorName = ApplicationContext.Arguments[1];

			if (!Context.GeneratorExists)
			{
				WriteLine("Generator '{0}' not found.", Context.GeneratorName);
				return 1;
			}

			try
			{
				Directory.Delete(Context.GeneratorDirectory, true);
				WriteLine("Uninstalled generator '{0}'.", Context.GeneratorName);

				if (File.Exists(Context.GeneratorConfigFile))
				{
					File.Delete(Context.GeneratorConfigFile);
				}
				return 0;
			}
			catch (IOException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          Context.GeneratorName, Context.GeneratorDirectory, e.Message);
			}
			catch (SecurityException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          Context.GeneratorName, Context.GeneratorDirectory, e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          Context.GeneratorName, Context.GeneratorDirectory, e.Message);
			}
			return 1;
		}
	}
}