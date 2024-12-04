using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Styling;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Microsoft.Win32;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Extensions;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Helper.HardwareInfo;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Models.Settings;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Helpers;
using SmartGenealogy.Languages;
using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views.Dialogs;
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

    private readonly ServiceManager<ViewModelBase> dialogFactory;


    public SharedState SharedState { get; }

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

    [ObservableProperty]
    private NumberFormatMode selectedNumberFormatMode;

    public IReadOnlyList<NumberFormatMode> NumberFormatModes { get; } =
        Enum.GetValues<NumberFormatMode>().Where(mode => mode != default).ToList();



    #region System Settings

    [ObservableProperty]
    private bool isWindowsLongPathsEnabled;

    #endregion

    #region System Info

    private static Lazy<IReadOnlyList<GpuInfo>> GpuInfosLazy { get; } =
        new(() => HardwareHelper.IterGpuInfo().ToImmutableArray());

    public static IReadOnlyList<GpuInfo> GpuInfos => GpuInfosLazy.Value;

    [ObservableProperty]
    private MemoryInfo memoryInfo;

    private readonly DispatcherTimer hardwareInfoUpdateTimer =
        new() { Interval = TimeSpan.FromSeconds(2.627) };

    public Task<CpuInfo> CpuInfoAsync => HardwareHelper.GetCpuInfoAsync();

    #endregion



    // Info section
    private const int VersionTapCountThreshold = 7;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(VersionFlyoutText))]
    private int versionTapCount;

    [ObservableProperty]
    private bool isVersionTapTeachingTipOpen;
    public string VersionFlyoutText =>
        $"You are {VersionTapCountThreshold - VersionTapCount} clicks away from enabling Debug options.";

    public string DataDirectory => settingsManager.IsLibraryDirSet ? settingsManager.LibraryDir : "Not set";

    public MainSettingsViewModel(
        INotificationService notificationService,
        ISettingsManager settingsManager,

        ServiceManager<ViewModelBase> dialogFactory,

        SharedState sharedState)
    {
        this.notificationService = notificationService;
        this.settingsManager = settingsManager;

        this.dialogFactory = dialogFactory;

        SharedState = sharedState;

        if (Program.Args.DebugMode)
        {
            SharedState.IsDebugMode = true;
        }

        SelectedTheme = settingsManager.Settings.Theme ?? AvailableThemes[1];
        SelectedLanguage = Cultures.GetSupportedCultureOrDefault(settingsManager.Settings.Language);

        settingsManager.RelayPropertyFor(this, vm => vm.SelectedTheme, settings => settings.Theme);



        settingsManager.RelayPropertyFor(
            this,
            vm => vm.SelectedNumberFormatMode,
            settings => settings.NumberFormatMode,
            true);

        //DebugThrowAsyncExceptionCommand.WithNotificationErrorHandler(notificationService, LogLevel.Warn);

        hardwareInfoUpdateTimer.Tick += OnHardwareInfoUpdateTimerTick;
    }

    /// <inheritdoc />
    public override void OnLoaded()
    {
        base.OnLoaded();

        hardwareInfoUpdateTimer.Start();

        if (Compat.IsWindows)
        {
            UpdateRegistrySettings();
        }
    }

    /// <inheritdoc />
    public override void OnUnloaded()
    {
        base.OnUnloaded();

        hardwareInfoUpdateTimer.Stop();
    }

    /// <inheritdoc />
    public override async Task OnLoadedAsync()
    {
        await base.OnLoadedAsync();

        //await notificationService.TryAsync(completionProvider.Setup());

        //// Start accounts update
        //accountsService
        //    .RefreshAsync()
        //    .SafeFireAndForget(ex =>
        //    {
        //        Logger.Error(ex, "Failed to refresh accounts");
        //        notificationService.ShowPersistent(
        //            "Failed to update account status",
        //            ex.ToString(),
        //            NotificationType.Error);
        //    });
    }

    [SupportedOSPlatform("windows")]
    private void UpdateRegistrySettings()
    {
        try
        {
            using var fsKey =
                Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentcontrolSet\Control\FileSystem")
                ?? throw new InvalidOperationException(
                    "Could not open registry key 'SYSTEM\\CurrentControlSet\\Control\\FileSystem'");

            IsWindowsLongPathsEnabled = Convert.ToBoolean(fsKey.GetValue("LongPathsEnabled", null));
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not read registry settings");
            notificationService.Show("Could not read registry settings", e.Message, NotificationType.Error);
        }
    }

    private void OnHardwareInfoUpdateTimerTick(object? sender, EventArgs e)
    {
        if (HardwareHelper.IsMemoryInfoAvailable && HardwareHelper.TryGetMemoryInfo(out var newMemoryInfo))
        {
            MemoryInfo = newMemoryInfo;
        }

        // Stop timer if live memory info is not available
        if (!HardwareHelper.IsLiveMemoryUsageInfoAvailable)
        {
            (sender as DispatcherTimer)?.Stop();
        }
    }

    partial void OnSelectedThemeChanged(string? value)
    {
        // In case design / tests
        if (Application.Current is null)
            return;
        // Change theme
        Application.Current.RequestedThemeVariant = value switch
        {
            "Dark" => ThemeVariant.Dark,
            "Light" => ThemeVariant.Light,
            _ => ThemeVariant.Default
        };
    }

    partial void OnSelectedLanguageChanged(CultureInfo? oldValue, CultureInfo newValue)
    {
        if (oldValue is null || newValue.Name == Cultures.Current?.Name)
            return;

        // Set locale
        if (AvailableLanguages.Contains(newValue))
        {
            Logger.Info("Changing language from {Old} to {New}", oldValue, newValue);

            Cultures.TrySetSupportedCulture(newValue, settingsManager.Settings.NumberFormatMode);
            settingsManager.Transaction(s => s.Language = newValue.Name);

            var dialog = new BetterContentDialog
            {
                Title = Resources.Label_RelaunchRequired,
                Content = Resources.Text_RelaunchRequiredToApplyLanguage,
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = Resources.Action_Relaunch,
                CloseButtonText = Resources.Action_RelaunchLater
            };

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    // Start the new app while passing our own PID to wait for exit
                    Process.Start(Compat.AppCurrentPath, $"--wait-for-exit-pid {Environment.ProcessId}");

                    // Shutdown the current app
                    App.Shutdown();
                }
            });
        }
        else
        {
            Logger.Info("Requested invalid language change from {Old} to {New}", oldValue, newValue);
        }
    }



    #region System

    /// <summary>
    /// Adds Smart Genealogy to Start Menu for the current user.
    /// </summary>
    [RelayCommand]
    private async Task AddToStartMenu()
    {
        if (!Compat.IsWindows)
        {
            notificationService.Show("Not supported", "This feature is only supported on Windows.");
            return;
        }

        await using var _ = new MinimumDelay(200, 300);

        var shortcutDir = new DirectoryPath(
            Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
            "Programs");
        var shortcutLink = shortcutDir.JoinFile("Smart Genealogy.lnk");

        var appPath = Compat.AppCurrentPath;
        var iconPath = shortcutDir.JoinFile("Smart Genealogy.ico");
        //await Assets.AppIcon.ExtractTo(iconPath);

        //WindowsShortcuts.CreateShortcut(shortcutLink, appPath, iconPath, "Smart Genealogy");

        notificationService.Show(
            "Added to Start Menu",
            "Smart Genealogy has been added to the Start Menu.",
            NotificationType.Success);
    }

    [RelayCommand]
    private async Task AddToGlobalStartMenu()
    {

    }

    public async Task PickNewDataDirectory()
    {
        var viewModel = dialogFactory.Get<SelectDataDirectoryViewModel>();
        var dialog = new BetterContentDialog
        {
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            IsFooterVisible = false,
            Content = new SelectDataDirectoryDialog { DataContext = viewModel }
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            // 1. For portable mode, call settings.SetPortableMode()
            if (viewModel.IsPortableMode)
            {
                settingsManager.SetPortableMode();
            }
            // 2. For custom path, call settings.SetLibraryPath(path)
            else
            {
                settingsManager.SetLibraryPath(viewModel.DataDirectory);
            }

            // Restart
            var restartDialog = new BetterContentDialog
            {
                Title = Resources.Label_RestartRequired,
                Content = Resources.Text_RestartRequired,
                PrimaryButtonText = Resources.Action_Restart,
                DefaultButton = ContentDialogButton.Primary,
                IsSecondaryButtonEnabled = false,
            };
            await restartDialog.ShowAsync();

            Process.Start(Compat.AppCurrentPath);
            App.Shutdown();
        }
    }

    #endregion



    #region Systems Setting Section

    [RelayCommand]
    private async Task OnWindowsLongPathsToggleClick()
    {
        if (!Compat.IsWindows)
            return;

        // Command is called after value is set, so if false we need to disable
        var requestedValue = IsWindowsLongPathsEnabled;

        try
        {
            var result = await WindowsElevated.SetRegistryValue(
                @"HKLM\SYSTEM\CurrentControlSet\Control\FileSystem",
                @"LongPathsEnabled",
                requestedValue ? 1 : 0);

            if (result != 0)
            {
                notificationService.Show(
                    "Failed to toggle long paths",
                    $"Error code: {result}",
                    NotificationType.Error);
                return;
            }

            await new BetterContentDialog
            {
                Title = Resources.Label_ChangesApplied,
                Content = Resources.Text_RestartMayBeRequiredForSystemChanges,
                CloseButtonText = Resources.Action_Close
            }.ShowAsync();
        }
        catch (Win32Exception e)
        {
            if (e.Message.EndsWith(
                @"The operation was canceled by the user.",
                StringComparison.OrdinalIgnoreCase))
                return;

            notificationService.Show("Failed to toggle long paths", e.Message, NotificationType.Error);
        }
        finally
        {
            UpdateRegistrySettings();
        }
    }

    #endregion

    #region Info Section

    public void OnVersionClick()
    {
        // Ignore if already enabled
        if (SharedState.IsDebugMode)
            return;

        VersionTapCount++;

        switch (VersionTapCount)
        {
            // Reached required threshold
            case >= VersionTapCountThreshold:
                {
                    IsVersionTapTeachingTipOpen = false;
                    // Enable debug options
                    SharedState.IsDebugMode = true;
                    notificationService.Show(
                        "Debug options enabled",
                        "Warning: Improper use may corrupt application state or cause loss of data.");
                    VersionTapCount = 0;
                    break;
                }
            // Open teaching tip above 3rd click
            case >= 3:
                IsVersionTapTeachingTipOpen = true;
                break;
        }
    }

    #endregion
}