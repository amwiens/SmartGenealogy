using FluentIcons.Common;

using Injectio.Attributes;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(SettingsPage))]
[RegisterSingleton<SettingsViewModel>]
public partial class SettingsViewModel : PageViewModelBase
{
    public override string Title => "Settings";
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Settings, IconVariant = IconVariant.Filled };
}