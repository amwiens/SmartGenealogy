using System;

namespace SmartGenealogy.Models;

/// <summary>
///
/// </summary>
[Flags]
public enum ViewModelState : uint
{
    /// <summary>
    /// View Model has been initially loaded
    /// </summary>
    InitialLoaded = 1 << 0,
}