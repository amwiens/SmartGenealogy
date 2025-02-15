namespace SmartGenealogy.Models;

[Flags]
public enum ViewModelState : uint
{
    /// <summary>
    /// View Model has been initially loaded
    /// </summary>
    InitialLoaded = 1 << 0,
}