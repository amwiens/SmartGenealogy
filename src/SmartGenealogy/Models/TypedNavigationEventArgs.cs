using System;

namespace SmartGenealogy.Models;

public class TypedNavigationEventArgs : EventArgs
{
    public Type ViewModelType { get; init; }

    public object? ViewModel { get; init; }
}