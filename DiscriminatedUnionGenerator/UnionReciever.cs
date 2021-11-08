using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiscriminatedUnionGenerator
{
    class UnionReciever : ISyntaxReceiver
    {
        public HashSet<EnumDeclarationSyntax> EnumDeclarations { get; } = new();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if(syntaxNode is AttributeSyntax attribute)
            {
                if ( attribute.Name.ToString().Contains(UnionGenerator.AttributeName) 
                    && attribute.Parent is AttributeListSyntax attributes)
                {
                   if(attributes.Parent is EnumMemberDeclarationSyntax members)
                    {
                        if(members.Parent is EnumDeclarationSyntax @enum)
                        {
                            EnumDeclarations.Add(@enum);
                        }
                    }
                }
            }
        }
    }
}
