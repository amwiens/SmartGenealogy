using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using FluentAvalonia.UI.Windowing;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.ViewModels.Dialogs;

namespace SmartGenealogy.Views.Dialogs;

[Transient]
public partial class ExceptionDialog : AppWindowBase
{
    public ExceptionDialog()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void CopyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var content = (DataContext as ExceptionViewModel)?.FormatAsMarkdown();

        if (content is not null && Clipboard is not null)
        {
            await Clipboard.SetTextAsync(content);
        }
    }

    private void ExitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}