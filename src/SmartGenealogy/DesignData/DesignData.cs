using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using SmartGenealogy.Core.Models.Progress;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.ViewModels.Settings;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.DesignData;

[Localizable(false)]
public static class DesignData
{
    [NotNull]
    public static IServiceProvider? Services { get; set; }

    private static bool isInitialized;

    // This needs to be static method instead of static constructor or else Avalonia analyzers won't work.
    public static void Initialize()
    {
        if (isInitialized)
            throw new InvalidOperationException("DesignData is already initialized.");

        var services = App.ConfigureServices();

        var activePackageId = Guid.NewGuid();


        // General services
        services
            .AddLogging();


        Services = services.BuildServiceProvider();

        var dialogFactory = Services.GetRequiredService<ServiceManager<ViewModelBase>>();
        var settingsManager = Services.GetRequiredService<ISettingsManager>();

        var notificationService = Services.GetRequiredService<INotificationService>();
    }

    public static ServiceManager<ViewModelBase> DialogFactory =>
        Services.GetRequiredService<ServiceManager<ViewModelBase>>();

    public static MainWindowViewModel MainWindowViewModel =>
        Services.GetRequiredService<MainWindowViewModel>();



    public static SettingsViewModel SettingsViewModel => Services.GetRequiredService<SettingsViewModel>();



    public static MainSettingsViewModel MainSettingsViewModel =>
        Services.GetRequiredService<MainSettingsViewModel>();



    public static SelectDataDirectoryViewModel SelectDataDirectoryViewModel =>
        Services.GetRequiredService<SelectDataDirectoryViewModel>();



    public static RefreshBadgeViewModel RefreshBadgeViewModel => new() { State = ProgressState.Success };
}