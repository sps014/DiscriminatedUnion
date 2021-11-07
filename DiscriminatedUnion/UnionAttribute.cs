using System;
namespace DiscriminatedUnion;

[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public class UnionAttribute : Attribute
{
    public UnionAttribute(string name)
    {

    }
}
