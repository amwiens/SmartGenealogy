namespace SmartGenealogy.Avalonia.Mvvm.Core;

[AttributeUsage(AttributeTargets.All)]
public class DoNotLogAttribute : Attribute { }

/// <summary>
/// Bindable class, aka a View Model.
/// </summary>
/// <remarks>All bound properties are held in a dictionary.</remarks>
public class Bindable : NotifyPropertyChanged
{
}