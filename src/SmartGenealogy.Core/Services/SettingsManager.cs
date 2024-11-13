using Microsoft.Extensions.Logging;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

[Singleton(typeof(ISettingsManager))]
public class SettingsManager(ILogger<SettingsManager> logger) : ISettingsManager
{

    public Settings Settings { get; private set; } = new();

}