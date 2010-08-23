using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;

namespace Poplar.Dsl
{
	/// <summary>
	/// Converts all macros that are not named 'template' to a macro named 'template'
	/// with the name of the macro as its sole argument.
	/// </summary>
	internal class ConvertMacroCompilerStep : AbstractTransformerCompilerStep
	{
		public override void Run()
		{
			Visit(CompileUnit);
		}

		public override void OnMacroStatement(MacroStatement node)
		{
            Console.WriteLine("ConvertMacroCompilerStep: {0}", node.Name);

			if (node.Name == "template")
			{
				return;
			}

			var macro = new MacroStatement {Name = "template"};
			macro.Arguments.Add(new StringLiteralExpression(node.Name));
			macro.Block = node.Block;
			ReplaceCurrentNode(macro);
		}
	}
}