using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Labs.Controls;
using Avalonia.Xaml.Interactions.Custom;

using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Languages;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.ViewModels.Progress;
using SmartGenealogy.Views;

namespace SmartGenealogy.ViewModels;

[View(typeof(MainWindow))]
public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    [ObservableProperty]
    private PageViewModelBase? currentPage;

    [ObservableProperty]
    private object? selectedCategory;

    [ObservableProperty]
    private List<PageViewModelBase> pages = new();

    [ObservableProperty]
    private List<PageViewModelBase> footerPages = new();

    public ProgressManagerViewModel ProgressManagerViewModel { get; init; }
    public UpdateViewModel UpdateViewModel { get; init; }

    public double PaneWidth =>
        Cultures.Current switch
        {
            _ => 200
        };

    public MainWindowViewModel()
    {

    }

    public override void OnLoaded()
    {
        base.OnLoaded();
    }

    protected override async Task OnInitialLoadedAsync()
    {
        await base.OnLoadedAsync();

        // Skip if design mode
        if (Design.IsDesignMode)
            return;


    }



    public async Task ShowUpdateDialog()
    {
        //var viewModel = dialogFactory.Get<UpdateViewModel>();
        //var dialog = new BetterContentDialog
        //{
        //    ContentVerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
        //    DefaultButton = ContentDialogButton.Close,
        //    IsPrimaryButtonEnabled = false,
        //    IsSecondaryButtonEnabled = false,
        //    IsFooterVisible = false,
        //    Content = new UpdateDialog { DataContext = viewModel }
        //};

        //await viewModel.Preload();
        //await dialog.ShowAsync();
    }
}