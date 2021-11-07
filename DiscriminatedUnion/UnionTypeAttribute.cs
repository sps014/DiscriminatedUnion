using System;
using System.Collections.Generic;
using System.Text;

namespace DiscriminatedUnion;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class UnionTypeAttribute<T> : Attribute
{
    public UnionTypeAttribute(string name)
    {

    }
}