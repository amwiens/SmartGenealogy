using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

public interface ISettingsManager
{
    Settings Settings { get; }
}