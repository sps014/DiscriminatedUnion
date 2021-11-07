using DiscriminatedUnion;

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
    [Union("Enum")]
    public enum Type
    {
        [UnionType<int>("Radius")]
        Circle,

        [UnionType<double>("Width")]
        [UnionType<double>("Height")]
        Rectangle
    }
}
