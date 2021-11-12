using System;
namespace DiscriminatedUnion;

[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public class UnionAttribute: Attribute
{
    public UnionAttribute(string name,Modifier specialModifiers=Modifier.Default
        ,ContainerType type=ContainerType.Struct)
    {

    }
    public enum Modifier
    {
        Default,
        Partial,
        Sealed,
        SealedPartial
    }
    public enum ContainerType
    {
        Struct,
        Class
    }


}

