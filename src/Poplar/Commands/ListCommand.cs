namespace Poplar.Commands
{
	public class ListCommand : Command
	{
		public override int Execute()
		{
			foreach (var generator in Context.Generators)
			{
				WriteLine(generator.Name);
			}
			return 0;
		}
	}
}