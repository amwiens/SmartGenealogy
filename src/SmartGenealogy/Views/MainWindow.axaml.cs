using System;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

using FluentAvalonia.UI.Controls;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.ViewModels;

namespace SmartGenealogy.Views;

[Singleton]
public partial class MainWindow : AppWindowBase
{

    private FlyoutBase? progressFlyout;

    public MainWindow()
    {
        InitializeComponent();


        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = FluentAvalonia.UI.Windowing.TitleBarHitTestType.Complex;
    }


    private void NavigationView_OnItemInvoked(object sender, NavigationViewItemInvokedEventArgs e)
    {

    }



    private void FooterDownloadItem_OnTapped(object? sender, TappedEventArgs e)
    {
        var item = sender as NavigationViewItem;
        var flyout = item!.ContextFlyout;
        flyout!.ShowAt(item);

        progressFlyout = flyout;
    }

    private async void FooterUpdateItem_OnTapped(object? sender, TappedEventArgs e)
    {
        // show update window thing
        if (DataContext is not MainWindowViewModel vm)
        {
            throw new NullReferenceException("DataContext is not MainWindowViewModel");
        }


    }

    private void FooterDiscordItem_OnTapped(object? sender, TappedEventArgs e)
    {

    }

    private void PatreonPatreonItem_OnTapped(object? sender, TappedEventArgs e)
    {

    }

    private void TopLevel_OnBackRequested(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavigationView_OnBackRequested(object? sender, NavigationViewBackRequestedEventArgs e)
    {

    }
}