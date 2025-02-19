using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Styling;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Extensions;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models.Settings;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Languages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views.Dialogs;
using SmartGenealogy.Views.Settings;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(MainSettingsPage))]
[ManagedService]
[RegisterSingleton<MainSettingsViewModel>]
public partial class MainSettingsViewModel : PageViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly INotificationService notificationService;
    private readonly ISettingsManager settingsManager;

    private readonly ServiceManager<ViewModelBase> dialogFactory;

    private readonly INavigationService<SettingsViewModel> settingsNavigationService;



    public bool IsMacOS => Compat.IsMacOS;

    public override string Title => "Settings";

    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Settings, IconVariant = IconVariant.Filled };

    public string AppVersion =>
        $"Version {Compat.AppVersion.ToDisplayString()}" + (Program.IsDebugBuild ? " (Debug)" : "");

    // Theme section
    [ObservableProperty]
    private string? selectedTheme;

    public IReadOnlyList<string> AvailableThemes { get; } = new[] { "Light", "Dark", "System" };

    [ObservableProperty]
    private CultureInfo selectedLanguage;

    public IReadOnlyList<CultureInfo> AvailableLanguages => Cultures.SupportedCultures;

    [ObservableProperty]
    private NumberFormatMode selectedNumberFormatMode;

    public IReadOnlyList<NumberFormatMode> NumberFormatModes { get; } =
        Enum.GetValues<NumberFormatMode>().Where(mode => mode != default).ToList();

    public IReadOnlyList<float> AnimationScaleOptions { get; } =
        new[] { 0f, 0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 1.75f, 2f };

    [ObservableProperty]
    private float selectedAnimationScale;


    public string DataDirectory => settingsManager.IsLibraryDirSet ? settingsManager.LibraryDir : "Not set";


    public MainSettingsViewModel(
        INotificationService notificationService,
        ISettingsManager settingsManager,

        ServiceManager<ViewModelBase> dialogFactory,

        INavigationService<SettingsViewModel> settingsNavigationService)
    {
        this.notificationService = notificationService;
        this.settingsManager = settingsManager;

        this.dialogFactory = dialogFactory;

        this.settingsNavigationService = settingsNavigationService;

        if (Program.Args.DebugMode)
        {

        }

        SelectedTheme = settingsManager.Settings.Theme ?? AvailableThemes[1];
        SelectedLanguage = Cultures.GetSupportedCultureOrDefault(settingsManager.Settings.Language);

        settingsManager.RelayPropertyFor(this, vm => vm.SelectedTheme, settings => settings.Theme);



        settingsManager.RelayPropertyFor(
            this,
            vm => vm.SelectedAnimationScale,
            settings => settings.AnimationScale);



        settingsManager.RelayPropertyFor(
            this,
            vm => vm.SelectedNumberFormatMode,
            settings => settings.NumberFormatMode,
            true);
    }



    partial void OnSelectedThemeChanged(string? value)
    {
        // In case design / tests
        if (Application.Current is null)
        {
            return;
        }
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
                //settingsManager.SetPortableMode();
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
}