# DiscriminatedUnion
 High performance Compile Time Discriminated Unions for C# , Rust and F# inspired
 
[Nuget](https://www.nuget.org/packages/TinyUnions) reference
```
dotnet add package TinyUnions
```

eg.
```cs
//code you write to create enum
[Union("ExpressionSyntax")]
public enum ExpressionSyntaxType
{
    [UnionProperty<SyntaxToken>("Number")]
    NumberExpressionSyntax,

    [UnionProperty<ExpressionSyntax>("Left")]
    [UnionProperty<SyntaxToken>("Operator")]
    [UnionProperty<ExpressionSyntax>("Right")]
    BinaryExpressionSyntax
}
```
equivalent Code as compared to 
```rs
enum ExpressionSyntax
{
   NumberExpressionSyntax(SyntaxToken Number),
   BinaryExpressionSyntax(ExpressionSyntax Left,SyntaxToken Operator,ExpressionSyntax Right);
}
```


Pattern Matching 
```cs
//create instance
ExpressionSyntax syntax = new ExpressionSyntax.NumberExpressionSyntax(new SyntaxToken(Value: "jkkjkj"));

if(syntax is ExpressionSyntax.NumberExpressionSyntax num)
{
   Console.WriteLine(num.Number);
}
else if(syntax is ExpressionSyntax.BinaryExpressionSyntax expr)
{
     Console.WriteLine(expr.Left+" "+ expr.Right);
}

//or
switch(syntax.Is)
{
    case ExpressionSyntaxType.NumberExpressionSyntax:
        var num=syntax.AsNumberExpressionSyntax();
        Console.WriteLine("Number Expression"+num.Number);
        break;
    case ExpressionSyntaxType.BinaryExpressionSyntax:
        Console.WriteLine("Binary Expression");
        break;
}
```
