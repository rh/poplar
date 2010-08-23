using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;

namespace Poplar.Dsl
{
    /// <summary>
    /// Converts all ReferenceExpressions that are arguments to the macros to a StringLiteralExpression.
    /// </summary>
    internal class ConvertReferenceToStringLiteralCompilerStep : AbstractTransformerCompilerStep
    {
        private readonly string[] macros;

        public ConvertReferenceToStringLiteralCompilerStep(params string[] macros)
        {
            this.macros = macros;
        }

        public override void Run()
        {
            Visit(CompileUnit);
        }

        public override void OnReferenceExpression(ReferenceExpression node)
        {
            Console.WriteLine("ConvertReferenceToStringLiteralCompilerStep: {0}", node.Name);
            var macro = node.GetAncestor(NodeType.MacroStatement) as MacroStatement;
            if (macro != null)
            {
                if (Array.Exists(macros, value => macro.Name == value))
                {
                    Console.WriteLine("ConvertReferenceToStringLiteralCompilerStep: replacing {0} at line {1}", node.Name, node.LexicalInfo.Line);
                    ReplaceCurrentNode(new StringLiteralExpression(node.Name));
                }
            }
        }
    }
}