using DiscriminatedUnion;

UType u=new UType.Circle(6);

if( u is UType.Circle c)
{
    Console.WriteLine(c.Radius);
}
else if(u is UType.Rectangle r)
{
    Console.WriteLine(r.Width*r.Height);
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
