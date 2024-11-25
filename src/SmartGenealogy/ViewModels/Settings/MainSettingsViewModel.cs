using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Extensions;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Languages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Settings;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(MainSettingsPage))]
[Singleton, ManagedService]
public partial class MainSettingsViewModel : PageViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly INotificationService notificationService;
    private readonly ISettingsManager settingsManager;



    public bool IsMacOS => Compat.IsMacOS;

    public override string Title => "Settings";
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Settings, IconVariant = IconVariant.Filled };

    public string AppVersion =>
        $"Version {Compat.AppVersion.ToDisplayString()}" + (Program.IsDebugBuild ? " (Debug)" : "");

    // Theme section
    [ObservableProperty]
    private string? selectedTheme;

    public IReadOnlyList<string> AvailableThemes { get; } = new[] { "Light", "Dark", "System", };

    [ObservableProperty]
    private CultureInfo selectedLanguage;

    public IReadOnlyList<CultureInfo> AvailableLanguages => Cultures.SupportedCultures;



    // Info section
    private const int VersionTapCountThreshold = 7;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(VersionFlyoutText))]
    private int versionTapCount;

    [ObservableProperty]
    private bool isVersionTapTeachingTipOpen;
    public string VersionFlyoutText =>
        $"You are {VersionTapCountThreshold - VersionTapCount} clicks away from enabling Debug options.";



    #region Info Section

    public void OnVersionClick()
    {
        //// Ignore if already enabled
        //if (SharedState.IsDebugMode)
        //    return;

        //VersionTapCount++;

        //switch (VersionTapCount)
        //{
        //    // Reached required threshold
        //    case >= VersionTapCountThreshold:
        //        {
        //            IsVersionTapTeachingTipOpen = false;
        //            // Enable debug options
        //            SharedState.IsDebugMode = true;
        //            notificationService.Show(
        //                "Debug options enabled",
        //                "Warning: Improper use may corrupt application state or cause loss of data.");
        //            VersionTapCount = 0;
        //            break;
        //        }
        //    // Open teaching tip above 3rd click
        //    case >= 3:
        //        IsVersionTapTeachingTipOpen = true;
        //        break;
        //}
    }

    #endregion
}