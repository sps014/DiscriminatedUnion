using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscriminatedUnionGenerator;

[Generator]
public class UnionGenerator : ISourceGenerator
{
    private const string Namespace = "DiscriminatedUnion";
    public const string AttributeName = "UnionProperty";
    private int nameCount = 0;
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not UnionReciever union)
            return;

        nameCount = 0;
        foreach (var enumDeclaration in union.EnumDeclarations)
            SaveEnumContext(enumDeclaration,ref context);

    }
    private void SaveEnumContext(EnumDeclarationSyntax @enum, ref GeneratorExecutionContext context)
    {
        WriteNameSpace(@enum, ref context);
    }

    private void WriteNameSpace(EnumDeclarationSyntax @enum, ref GeneratorExecutionContext context)
    {
        using StringWriter ss = new();
        using IndentedTextWriter writer = new(ss);

        var usings = GetUsings(@enum);
        writer.WriteLine(usings);
        var nameSpace = GetEnumNameSpace(@enum);
        writer.WriteLine($"namespace {nameSpace}");
        writer.WriteLine("{");
        writer.Indent++;

        WriteEnumClass(@enum, writer);

        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        var enumName=GetEnumName(@enum);
        Console.WriteLine(ss.ToString());
        context.AddSource($"{enumName}_{nameCount++}.cs", SourceText.From(ss.ToString(), System.Text.Encoding.UTF8));
    }
    private void WriteEnumClass(EnumDeclarationSyntax @enum, IndentedTextWriter writer)
    {
        var DuName=GetEnumName(@enum);
        var modifier=GetModifier(@enum);

        var partialModifier=GetSpecialModifiers(@enum).Contains("partial")?"partial":"";
        writer.WriteLine($"{modifier} {partialModifier} interface {DuName}");
        writer.WriteLine("{");
        writer.Indent++;

        WriteEnumMembers(@enum, writer,DuName);

        writer.Indent--;
        writer.WriteLine("}");
    }
    private void WriteEnumMembers(EnumDeclarationSyntax @enum,IndentedTextWriter writer,string DuName)
    {
        var containerType = GetContainerType(@enum);
        var specialModifier=GetSpecialModifiers(@enum);
        foreach (var member in @enum.Members)
        {
            var name=GetMemberName(member);
            var fields = GetMemberFields(member);
            var enumname = @enum.Identifier.ValueText;
            writer.WriteLine($"public {specialModifier} record {containerType} {name}({fields}):{DuName}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"public {enumname} Is=>{enumname}.{name};");
            writer.Indent--;
            writer.WriteLine("}");
            
        }
        WriteEnumIs(@enum,writer);
        WriteAsMethods(@enum,writer);
    }

    private void WriteAsMethods(EnumDeclarationSyntax @enum, IndentedTextWriter writer)
    {
        foreach (var member in @enum.Members)
        {
            var name = GetMemberName(member);
            writer.WriteLine($"public {name} As{name}()=>({name})this;");
        }
    }
    private void WriteEnumIs(EnumDeclarationSyntax @enum,IndentedTextWriter writer)
    {
        var ename = @enum.Identifier.ValueText;
        writer.WriteLine($"{ename} Is{{get;}}");
    }

    private string GetContainerType(EnumDeclarationSyntax syntax)
    {
        foreach (var att in syntax.AttributeLists)
        {
            foreach (var atk in att.Attributes)
            {
                if (!atk.Name.ToString().Equals("Union"))
                    continue;
                if (atk.ArgumentList.Arguments.Count < 3)
                    break;

                if (atk.ArgumentList.Arguments[2].Expression.ToString().Trim('"').Contains("Class"))
                    return "";
            }
        }
        return "struct";
    }
    private string GetSpecialModifiers(EnumDeclarationSyntax syntax)
    {
     
        foreach (var att in syntax.AttributeLists)
        {
            foreach (var atk in att.Attributes)
            {
                if (!atk.Name.ToString().Equals("Union"))
                    continue;
                if (atk.ArgumentList.Arguments.Count < 2)
                    break;

                var word = atk.ArgumentList.Arguments[1].Expression.ToString().Trim('"');
                if(word.Contains(DiscriminatedUnion.UnionAttribute.Modifier.SealedPartial.ToString()))
                    return "sealed partial";
               else if (word.Contains(DiscriminatedUnion.UnionAttribute.Modifier.Sealed.ToString()))
                    return "sealed";
                else if (word.Contains(DiscriminatedUnion.UnionAttribute.Modifier.Partial.ToString()))
                    return "partial";
            }
        }
        return "";
    }
    private string GetMemberFields(EnumMemberDeclarationSyntax member)
    {
        List<(string Type, string Name)> fields = new();

        foreach(var a in member.AttributeLists)
        {
            foreach(var attr in a.Attributes)
            {
                var typeArgs = attr.Name.DescendantNodes().OfType<TypeArgumentListSyntax>();
                if (!typeArgs.Any())
                    continue;

                var type=typeArgs.First().Arguments[0].GetText().ToString();

                var name=attr.ArgumentList.Arguments[0].Expression.ToString().Trim('"');
                fields.Add((type,name));
            }
        }
        var merged = fields.Select(f => $"{f.Type} {f.Name}");
        var strMerged = string.Join(",", merged);
        return strMerged;
    }
    private string GetMemberName(EnumMemberDeclarationSyntax mem)
    {
        return mem.Identifier.ValueText;
    }
    private string GetModifier(EnumDeclarationSyntax @enum)
    {
        return @enum.Modifiers.ToString();
    }
    private string GetUsings(EnumDeclarationSyntax @enum)
    {
        var usings = @enum.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
        return string.Join("\r\n",usings.Select(u=>u.ToFullString()));
    }
    private string GetEnumNameSpace(EnumDeclarationSyntax @enum)
    {
        if(@enum.Parent is NamespaceDeclarationSyntax @namespace)
            return @namespace.Name.ToString();

        if(@enum.Parent is FileScopedNamespaceDeclarationSyntax @file)
            return @file.Name.ToString();

        return Namespace;
    }

    private string GetEnumName(EnumDeclarationSyntax @enum)
    {
        foreach(var att in @enum.AttributeLists)
        {
            foreach(var atk in att.Attributes)
            {
                if (!atk.Name.ToString().Equals("Union"))
                    continue;
                return atk.ArgumentList.Arguments[0].Expression.ToString().Trim('"');
            }
        }
        return $"U{@enum.Identifier}";
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new UnionReciever());
    }
}