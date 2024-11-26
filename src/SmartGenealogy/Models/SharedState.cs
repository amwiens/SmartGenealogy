using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.Models;

/// <summary>
/// Singleton DI service for observable shared UI state.
/// </summary>
[Singleton]
public partial class SharedState : ObservableObject
{
    /// <summary>
    /// Whether debug mode enabled from settings page version tap.
    /// </summary>
    [ObservableProperty]
    private bool isDebugMode;
}