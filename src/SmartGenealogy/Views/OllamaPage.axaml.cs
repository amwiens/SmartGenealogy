using Injectio.Attributes;

namespace SmartGenealogy.Views;

[RegisterSingleton<OllamaPage>]
public partial class OllamaPage : UserControl
{
    public OllamaPage()
    {
        InitializeComponent();
    }

    private void ScrollViewer_OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (e.ExtentDelta.Y > 0)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;
            scrollViewer.ScrollToEnd();
        }
    }
}