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


        isInitialized = true;
    }



    public static ServiceManager<ViewModelBase> DialogFactory =>
        Services.GetRequiredService<ServiceManager<ViewModelBase>>();

    public static MainWindowViewModel MainWindowViewModel =>
        Services.GetRequiredService<MainWindowViewModel>();



    public static SelectDataDirectoryViewModel SelectDataDirectoryViewModel =>
        Services.GetRequiredService<SelectDataDirectoryViewModel>();



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