﻿using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace SmartGenealogy.Core.Attributes;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors), MeansImplicitUse]
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute : Attribute
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    public Type? InterfaceType { get; }

    public TransientAttribute() { }

    public TransientAttribute(Type interfaceType)
    {
        InterfaceType = interfaceType;
    }
}