using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using SmartGenealogy.Core.Models.Progress;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy.DesignData;

[Localizable(false)]
public static class DesignData
{
    [NotNull]
    public static IServiceProvider? Services { get; set; }

    private static bool isInitialized;

    // This needs to be static method instead of static constructor
    // or else Avalonia analyzers won't work.
    public static void Initialize()
    {
        if (isInitialized)
            throw new InvalidOperationException("DesignData is already initialized.");

        var services = App.ConfigureServices();

        services.AddSingleton<ISettingsManager, MockSettingsManager>();

        // General services
        services
            .AddLogging();

        // Mock services
        services
            .AddSingleton(Substitute.For<INotificationService>());

        // Placeholder services that nobody should need during design time


        // Override Launch page with mock


        Services = services.BuildServiceProvider();

        var dialogFactory = Services.GetRequiredService<ServiceManager<ViewModelBase>>();
        var settingsManager = Services.GetRequiredService(typeof(ISettingsManager));

        var notificationService = Services.GetRequiredService<INotificationService>();



        UpdateViewModel = Services.GetRequiredService<UpdateViewModel>();
        UpdateViewModel.CurrentVersionText = "v2.0.0";
        UpdateViewModel.NewVersionText = "v2.0.1";
        UpdateViewModel.ReleaseNotes = "## v2.0.1\n- Fixed a bug\n- Added a feature\n-Removed a feature";

        isInitialized = true;
    }



    [NotNull]
    public static UpdateViewModel? UpdateViewModel { get; private set; }

    public static ServiceManager<ViewModelBase> DialogFactory =>
        Services.GetRequiredService<ServiceManager<ViewModelBase>>();

    public static MainWindowViewModel MainWindowViewModel =>
        Services.GetRequiredService<MainWindowViewModel>();

    public static FirstLaunchSetupViewModel FirstLaunchSetupViewModel =>
        Services.GetRequiredService<FirstLaunchSetupViewModel>();



    public static SettingsViewModel SettingsViewModel => Services.GetRequiredService<SettingsViewModel>();



    public static MainSettingsViewModel MainSettingsViewModel =>
        Services.GetRequiredService<MainSettingsViewModel>();



    public static NotificationSettingsViewModel NotificationSettingsViewModel =>
        Services.GetRequiredService<NotificationSettingsViewModel>();



    public static SelectDataDirectoryViewModel SelectDataDirectoryViewModel =>
        Services.GetRequiredService<SelectDataDirectoryViewModel>();



    public static ExceptionViewModel ExceptionViewModel =>
        DialogFactory.Get<ExceptionViewModel>(viewModel =>
        {
            // Use try-catch to generate traceback information
            try
            {
                try
                {
                    throw new OperationCanceledException("Example");
                }
                catch (OperationCanceledException e)
                {
                    throw new AggregateException(e);
                }
            }
            catch (AggregateException e)
            {
                viewModel.Exception = e;
            }
        });



    public static RefreshBadgeViewModel RefreshBadgeViewModel => new() { State = ProgressState.Success };



    public static string CurrentDirectory => Directory.GetCurrentDirectory();

    public static Indexer Types { get; } = new();

    public class Indexer
    {
        private List<Type> types = new();

        public object? this[string typeName]
        {
            get
            {
                var type = Type.GetType(typeName);

                type ??= typeof(DesignData)
                    .Assembly.GetTypes()
                    .FirstOrDefault(t => (t.FullName ?? t.Name).EndsWith(typeName));

                if (type is null)
                {
                    throw new ArgumentException($"Type {typeName} not found");
                }

                try
                {
                    return Services.GetRequiredService(type);
                }
                catch (InvalidOperationException)
                {
                    return Activator.CreateInstance(type);
                }
            }
        }
    }
}