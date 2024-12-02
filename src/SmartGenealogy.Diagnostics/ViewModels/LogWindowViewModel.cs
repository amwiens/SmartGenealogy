using Microsoft.Extensions.DependencyInjection;

using SmartGenealogy.Diagnostics.LogViewer.Core.ViewModels;

namespace SmartGenealogy.Diagnostics.ViewModels;

public class LogWindowViewModel
{
    public LogViewerControlViewModel LogViewer { get; }

    public LogWindowViewModel(LogViewerControlViewModel logViewer)
    {
        LogViewer = logViewer;
    }

    public static LogWindowViewModel FromServiceProvider(IServiceProvider services)
    {
        return new LogWindowViewModel(services.GetRequiredService<LogViewerControlViewModel>());
    }
}