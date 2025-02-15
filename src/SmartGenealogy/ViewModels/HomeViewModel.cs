using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using FluentIcons.Common;

using Injectio.Attributes;

using SmartGenealogy.Messages;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(HomePage))]
[RegisterSingleton<HomeViewModel>]
public partial class HomeViewModel : PageViewModelBase
{
    public override string Title => "Home";

    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Home, IconVariant = IconVariant.Filled };

    [RelayCommand]
    private void OpenFile()
    {
        WeakReferenceMessenger.Default.Send(new FileOpenedMessage("test"));
    }
}