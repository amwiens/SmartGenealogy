using System;
using System.Threading.Tasks;

using FluentAvalonia.UI.Controls;
using FluentIcons.Common;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Languages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views;
using SmartGenealogy.Views.Dialogs;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(HomePage))]
[Singleton]
public partial class HomeViewModel : PageViewModelBase
{
    private readonly ServiceManager<ViewModelBase> dialogFactory;

    public override string Title => Resources.Label_Home;
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Home, IconVariant = IconVariant.Filled };

    public HomeViewModel(
        ServiceManager<ViewModelBase> dialogFactory)
    {
        this.dialogFactory = dialogFactory;
    }

    public async Task CreateNewProject()
    {
        var viewModel = dialogFactory.Get<CreateProjectViewModel>();
        var dialog = new BetterContentDialog
        {
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            IsFooterVisible = false,
            Content = new CreateProjectDialog { DataContext = viewModel }
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {

        }
    }
}