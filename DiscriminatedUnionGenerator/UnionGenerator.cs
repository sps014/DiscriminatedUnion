using Microsoft.CodeAnalysis;
using System;
using System.CodeDom.Compiler;

namespace DiscriminatedUnionGenerator
{
    [Generator]
    public class UnionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if(context.SyntaxContextReceiver is not UnionReciever union)
                return;

            foreach(var enumDeclaration in union.EnumDeclarations)
            {

            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() =>new UnionReciever());
        }
    }
}
