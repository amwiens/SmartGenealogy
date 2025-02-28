using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(PlacesPage))]
[RegisterSingleton<PlacesViewModel>]
public partial class PlacesViewModel : PageViewModelBase
{
    public override string Title => "Places";
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Map, IconVariant = IconVariant.Filled };

    public IReadOnlyList<PageViewModelBase> SubPages { get; }

    [ObservableProperty]
    private ObservableCollection<PageViewModelBase> currentPagePath = [];

    [ObservableProperty]
    private PageViewModelBase? currentPage;

    public PlacesViewModel()
    {
    }
}