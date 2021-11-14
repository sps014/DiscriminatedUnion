using DiscriminatedUnion;
using Namespacer;
using TestName;

UType u=new UType.Circle(6);

if( u is UType.Circle c)
    Console.WriteLine(c.Radius);

else if(u is UType.Rectangle r)
    Console.WriteLine(r.Height);

u = new UType.Rectangle(12, 10);
//or 
switch(u.Is)
{
    case Type.Circle:
        Console.WriteLine(u.AsCircle().Radius);
        break;
    case Type.Rectangle:
        Console.WriteLine(u.AsRectangle().Height);
        break;
}

ExpressionSyntax syntax = new ExpressionSyntax.NumberExpressionSyntax(new SyntaxToken(Value: "jkkjkj"));

switch(syntax.Is)
{
    case ExpressionSyntaxType.NumberExpressionSyntax:
        Console.WriteLine("Number Expression");
        break;
    case ExpressionSyntaxType.BinaryExpressionSyntax:
        Console.WriteLine("Binary Expression");
        break;
}

enum Type
{
    [UnionProperty<int>("Radius")]
    Circle,

    [UnionProperty<double>("Width")]
    [UnionProperty<double>("Height")]
    Rectangle
}
