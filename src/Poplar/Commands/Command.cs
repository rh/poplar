using Optional.Attributes;
using Optional.Commands;

namespace Poplar.Commands
{
	public abstract class Command : Optional.Commands.Command
	{
		/// <remarks>
		/// This property will be set after the command has been created by <see cref="CommandFactory"/>
		/// See <see cref="Program"/>.
		/// </remarks>
		[Ignore]
		public Context Context { get; set; }
	}
}