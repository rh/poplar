using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;

namespace Poplar.Dsl
{
	/// <summary>
	/// Converts all ReferenceExpressions that are arguments to the macros 'template' and
	/// 'parameter' to a StringLiteralExpression.
	/// </summary>
	internal class ConvertReferenceToStringLiteralCompilerStep : AbstractTransformerCompilerStep
	{
		public override void Run()
		{
			Visit(CompileUnit);
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			var ancestor = node.GetAncestor(NodeType.MacroStatement);
			var macro = ancestor as MacroStatement;
			if (macro != null)
			{
				if (macro.Name == "template" || macro.Name == "parameter")
				{
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));
				}
			}
		}
	}
}