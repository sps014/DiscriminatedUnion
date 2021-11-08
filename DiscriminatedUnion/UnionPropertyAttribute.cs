using System;
using System.Collections.Generic;
using System.Text;

namespace DiscriminatedUnion;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class UnionPropertyAttribute<T> : Attribute
{
    public UnionPropertyAttribute(string name)
    {

    }
}