using SmartGenealogy.Core.Models.Update;

namespace SmartGenealogy.Core.Updater;

public class UpdateStatusChangedEventArgs : EventArgs
{
    public UpdateInfo? LatestUpdate { get; init; }

    public IReadOnlyDictionary<UpdateChannel, UpdateInfo> UpdateChannels { get; init; } =
        new Dictionary<UpdateChannel, UpdateInfo>();

    public DateTimeOffset CheckedAt { get; init; } = DateTimeOffset.UtcNow;
}