using Injectio.Attributes;

using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

[RegisterSingleton<ISettingsManager, SettingsManager>]
public class SettingsManager/*(ILogger<SettingsManager> logger)*/ : ISettingsManager
{

    public Settings Settings { get; private set; } = new();
}