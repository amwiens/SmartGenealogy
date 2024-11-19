using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using FluentAvalonia.UI.Controls;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.Views;

[Singleton]
public partial class FirstLaunchSetupWindow : AppWindowBase
{
    public ContentDialogResult Result { get; private set; }

    public FirstLaunchSetupWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
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