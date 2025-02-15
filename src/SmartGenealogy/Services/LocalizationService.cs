using System;
using System.Globalization;
using System.Resources;
using System.Threading;

using Avalonia.Markup.Xaml;

namespace SmartGenealogy.Services;

public class LocalizationService : MarkupExtension
{
    private static readonly Lock Lock = new();
    private static ResourceManager? _resourceMan;
    private static readonly CultureInfo ResourceCulture = CultureInfo.CurrentUICulture;

    private static ResourceManager ResourceManager
    {
        get
        {
            if (_resourceMan != null)
                return _resourceMan;
            lock (Lock)
            {
                _resourceMan ??= new ResourceManager("SmartGenealogy.Assets.Localization.Resources",
                    typeof(LocalizationService).Assembly);
            }
            return _resourceMan;
        }
    }

    public static string GetString(string name)
    {
        return ResourceManager.GetString(name, ResourceCulture) ?? "[UNDEFINED_LOCALIZTION_KEY]";
    }

    // MarkupExtension
    public string Key { get; set; } = string.Empty;
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return GetString(Key);
    }
}