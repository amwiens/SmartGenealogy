using SmartGenealogy.Core.Models.Progress;
using SmartGenealogy.Core.Models.Update;

namespace SmartGenealogy.Core.Updater;

public interface IUpdateHelper
{
    event EventHandler<UpdateStatusChangedEventArgs>? UpdateStatusChanged;

    Task StartCheckingForUpdates();

    Task CheckForUpdate();

    Task DownloadUpdate(UpdateInfo updateInfo, IProgress<ProgressReport> progress);
}