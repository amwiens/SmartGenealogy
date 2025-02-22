using Avalonia.Interactivity;

using FluentAvalonia.UI.Windowing;

using Injectio.Attributes;

using SmartGenealogy.Controls;
using SmartGenealogy.ViewModels.Dialogs;

namespace SmartGenealogy.Views.Dialogs;

[RegisterTransient<ExceptionDialog>]
public partial class ExceptionDialog : AppWindowBase
{
    public ExceptionDialog()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }

    private async void CopyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var content = (DataContext as ExceptionViewModel)?.FormatAsMarkdown();

        if (content is not null && Clipboard is not null)
        {
            await Clipboard.SetTextAsync(content);
        }
    }

    private void ContinueButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExceptionViewModel viewModel)
        {
            viewModel.IsContinueResult = true;
        }

        Close();
    }

    private void ExitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}