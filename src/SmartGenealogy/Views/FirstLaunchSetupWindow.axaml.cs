using Avalonia.Interactivity;

using FluentAvalonia.UI.Controls;

using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views;

[RegisterSingleton<FirstLaunchSetupWindow>]
public partial class FirstLaunchSetupWindow : AppWindowBase
{
    public ContentDialogResult Result { get; private set; }

    public FirstLaunchSetupWindow()
    {
        InitializeComponent();
    }

    private void ContinueButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Primary;
        Close();
    }

    private void QuitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.None;
        Close();
    }
}