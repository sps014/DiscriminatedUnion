using DiscriminatedUnion;

enum Type
{
    [UnionType<int>("Radius")]
    Circle,

    [UnionType<double>("Width")]
    [UnionType<double>("Height")]
    Rectangle
}