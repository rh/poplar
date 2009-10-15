using System;
using System.IO;
using System.Security;
using Optional.Attributes;

namespace Poplar.Commands
{
	[Description("Uninstalls generators")]
	public class UninstallCommand : Command
	{
		[Required, Description("The generator to remove")]
		public string Generator { get; set; }

		public override int Execute()
		{
			GeneratorContext.GeneratorName = Generator;

			if (!GeneratorContext.GeneratorExists)
			{
				WriteLine("Generator '{0}' not found.", GeneratorContext.GeneratorName);
				return 1;
			}

			try
			{
				Directory.Delete(GeneratorContext.GeneratorDirectory, true);
				WriteLine("Uninstalled generator '{0}'.", GeneratorContext.GeneratorName);

				if (File.Exists(GeneratorContext.GeneratorConfigFile))
				{
					File.Delete(GeneratorContext.GeneratorConfigFile);
				}
				return 0;
			}
			catch (IOException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          GeneratorContext.GeneratorName, GeneratorContext.GeneratorDirectory, e.Message);
			}
			catch (SecurityException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          GeneratorContext.GeneratorName, GeneratorContext.GeneratorDirectory, e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				WriteLine("Failed to remove generator '{0}': you do not have the required permission to delete directory '{1}'. Details: {2}",
				          GeneratorContext.GeneratorName, GeneratorContext.GeneratorDirectory, e.Message);
			}
			return 1;
		}
	}
}