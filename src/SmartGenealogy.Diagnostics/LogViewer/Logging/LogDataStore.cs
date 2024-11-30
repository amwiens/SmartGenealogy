using Avalonia.Threading;

using SmartGenealogy.Diagnostics.LogViewer.Core.Logging;

namespace SmartGenealogy.Diagnostics.LogViewer.Logging;

public class LogDataStore : Core.Logging.LogDataStore
{
    #region Methods

    public override async void AddEntry(LogModel logModel) =>
        await Dispatcher.UIThread.InvokeAsync(() => base.AddEntry(logModel));

    #endregion
}