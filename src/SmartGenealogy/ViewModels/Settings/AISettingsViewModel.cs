using Avalonia.Threading;
using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Settings;
using SmartGenealogy.Services;
using SmartGenealogy.Animations;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(AISettingsPage))]
[RegisterSingleton<AISettingsViewModel>]
[ManagedService]
public partial class AISettingsViewModel : PageViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ISettingsManager settingsManager;
    private readonly INavigationService<SettingsViewModel> aiSettingsNavigationService;

    public override string Title => "AI Settings";
    public override IconSource IconSource => new SymbolIconSource { Symbol = Symbol.Alert, IconVariant = IconVariant.Filled };

    public AISettingsViewModel(
        ISettingsManager settingsManager,
        INavigationService<SettingsViewModel> aiSettingsNavigationService)
    {
        this.settingsManager = settingsManager;
        this.aiSettingsNavigationService = aiSettingsNavigationService;
    }

    [RelayCommand]
    private void NavigateToSubPage(Type viewModelType)
    {
        Dispatcher.UIThread.Post(
            () =>
                aiSettingsNavigationService.NavigateTo(
                    viewModelType,
                    BetterSlideNavigationTransition.PageSlideFromRight),
            DispatcherPriority.Send);
    }
}