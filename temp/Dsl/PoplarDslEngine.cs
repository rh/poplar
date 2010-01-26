using Boo.Lang.Compiler;
using Rhino.DSL;

namespace Poplar.Dsl
{
	public class PoplarDslEngine : DslEngine
	{
		protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls)
		{
			pipeline.Insert(1, new ConvertMacroCompilerStep());
			pipeline.Insert(2, new ConvertReferenceToStringLiteralCompilerStep());
			pipeline.Insert(3, new ImplicitBaseClassCompilerStep(typeof(PoplarTemplate), "Execute", "Poplar.Dsl"));
		}
	}
}