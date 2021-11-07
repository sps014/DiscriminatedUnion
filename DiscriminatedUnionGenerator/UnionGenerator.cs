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
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not UnionReciever union)
            return;

        using StringWriter ss = new();
        using IndentedTextWriter writer = new(ss);

        foreach (var enumDeclaration in union.EnumDeclarations)
            SaveEnumContext(enumDeclaration, writer);

        Console.WriteLine(ss.ToString());
        context.AddSource("generated_unions.cs", SourceText.From(ss.ToString(), System.Text.Encoding.UTF8));
    }
    private void SaveEnumContext(EnumDeclarationSyntax @enum, IndentedTextWriter writer)
    {
        WriteNameSpace(@enum, writer);
    }

    private void WriteNameSpace(EnumDeclarationSyntax @enum, IndentedTextWriter writer)
    {
        var nameSpace = GetEnumNameSpace(@enum);
        writer.WriteLine($"namespace {nameSpace}");
        writer.WriteLine("{");
        writer.Indent++;

        WriteEnumClass(@enum, writer);

        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
    }
    private void WriteEnumClass(EnumDeclarationSyntax @enum, IndentedTextWriter writer)
    {
        var DuName=GetEnumName(@enum);
        var modifier=GetModifier(@enum);
        writer.WriteLine($"{modifier} abstract record {DuName}");
        writer.WriteLine("{");
        writer.Indent++;

        WriteEnumMembers(@enum, writer,DuName);

        writer.Indent--;
        writer.WriteLine("}");
    }
    private void WriteEnumMembers(EnumDeclarationSyntax @enum,IndentedTextWriter writer,string DuName)
    {
        foreach(var member in @enum.Members)
        {
            var name=GetMemberName(member);
            var fields = GetMemberFields(member);
            var strMerged=string.Join(",",fields);
            writer.WriteLine($"public record {name}({fields}):{DuName};");
        }
    }

    private string GetMemberFields(EnumMemberDeclarationSyntax member)
    {
        List<(string Type, string Name)> fields = new();

        foreach(var a in member.AttributeLists)
        {
            foreach(var attr in a.Attributes)
            {
                var match = Regex.Match(attr.Name.ToString(), @"UnionType<([0-9a-zA-Z\[\]\(\)\s_\.]*)>");
                if (!match.Success)
                    continue;
                var type=match.Groups[1].Value;
                var name=attr.ArgumentList.Arguments[0].ToString().Trim('"');
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

    private string GetEnumNameSpace(EnumDeclarationSyntax @enum)
    {
        if(@enum.Parent is NamespaceDeclarationSyntax @namespace)
            return @namespace.Name.ToString();
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
                return atk.ArgumentList.Arguments[0].ToString().Trim('"');
            }
        }
        return $"U{@enum.Identifier}";
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new UnionReciever());
    }
}