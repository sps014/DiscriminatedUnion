using DiscriminatedUnion;
var u=new UType.Circle(6);
u.Radius = 32.0f;
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
