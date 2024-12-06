using SmartGenealogy.Core.Services;

namespace SmartGenealogy.Core.Models.Settings;

/// <summary>
/// Transaction object which saves project settings manager changes when disposed.
/// </summary>
public class ProjectSettingsTransaction(IProjectSettingsManager projectSettingsManager, Action onCommit, Func<Task> onCommitAsync)
    : IDisposable, IAsyncDisposable
{
    public ProjectSettings ProjectSettings => projectSettingsManager.Settings;

    public void Dispose()
    {
        onCommit();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await onCommitAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}