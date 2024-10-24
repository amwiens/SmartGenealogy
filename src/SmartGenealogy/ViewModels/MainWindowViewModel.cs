using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Core.Attributes;
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

    public double PaneWidth => 200;
        //Cultures.Current switch
        //{
        //    _ => 200
        //};
}
