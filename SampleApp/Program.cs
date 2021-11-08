using DiscriminatedUnion;

UType u=new UType.Circle(6);

if( u is UType.Circle c)
    Console.WriteLine(c.Radius);

else if(u is UType.Rectangle r)
    Console.WriteLine(r.Width*r.Height);
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
enum Type
{
    [UnionType<int>("Radius")]
    Circle,

    [UnionType<double>("Width")]
    [UnionType<double>("Height")]
    Rectangle
}

namespace Test
{
    [Union("Enumz")]
    public enum Type
    {
        [UnionType<int>("Radius")]
        Circle,

        [UnionType<double>("Width")]
        [UnionType<double>("Height")]
        Rectangle
    }
}
