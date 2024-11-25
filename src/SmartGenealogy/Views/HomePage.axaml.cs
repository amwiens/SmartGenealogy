using System;
using System.ComponentModel;

using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels;

namespace SmartGenealogy.Views;

[Singleton]
public partial class HomePage : UserControlBase, IHandleNavigation
{
    private readonly INavigationService<HomeViewModel> homeNavigationService;

    private bool hasLoaded;

    private HomeViewModel ViewModel => (HomeViewModel)DataContext!;

    [DesignOnly(true)]
    [Obsolete("For XAML use only", true)]
    public HomePage()
        : this(App.Services.GetRequiredService<INavigationService<HomeViewModel>>()) { }

    public HomePage(INavigationService<HomeViewModel> homeNavigationService)
    {
        this.homeNavigationService = homeNavigationService;

        InitializeComponent();
    }



    public bool GoBack()
    {
        return homeNavigationService.GoBack();
    }
}