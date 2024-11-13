using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGenealogy.Core.Models.Settings;

public class Settings
{
    public int? Version { get; set; } = 1;
    public bool FirstLaunchSetupComplete { get; set; }
    public string? Theme { get; set; } = "Dark";
    public string? Language { get; set; } = GetDefaultCulture().Name;



    public WindowSettings? WindowSettings { get; set; }



    /// <summary>
    /// Return either the system default culture, if supported, or en-US
    /// </summary>
    /// <returns></returns>
    public static CultureInfo GetDefaultCulture()
    {
        var supportedCultures = new[] { "en-US", "ja-JP", "zh-Hans", "zh-Hant" };

        var systemCulture = CultureInfo.InstalledUICulture;

        return supportedCultures.Contains(systemCulture.Name) ? systemCulture : new CultureInfo("en-US");
    }
}