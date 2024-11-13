using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AsyncAwaitBestPractices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Config;
using NLog.Extensions.Logging;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels;
using SmartGenealogy.Views;

using Application = Avalonia.Application;
using Logger = NLog.Logger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SmartGenealogy;

public sealed class App : Application
{
    private static readonly Lazy<Logger> LoggerLazy = new(LogManager.GetCurrentClassLogger);
    private static Logger Logger => LoggerLazy.Value;

    private readonly SemaphoreSlim onExitSemaphore = new(1, 1);

    private bool IsAsyncDisposeComplete;

    private bool isOnExitComplete;

    [NotNull]
    public static IServiceProvider? Services { get; private set; }

    [NotNull]
    public static Visual? VisualRoot { get; internal set; }

    public static TopLevel TopLevel => TopLevel.GetTopLevel(VisualRoot)!;

    internal static bool IsHeadlessMode =>
        TopLevel.TryGetPlatformHandle()?.HandleDescriptor is null or "STUB";

    [NotNull]
    public static IStorageProvider? StorageProvider { get; internal set; }

    [NotNull]
    public static IClipboard? Clipboard { get; internal set; }

    [NotNull]
    public static IConfiguration? Config { get; private set; }

    public IClassicDesktopStyleApplicationLifetime? DesktopLifetime =>
        ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

    public static new App? Current => (App?)Application.Current;

    /// <summary>
    /// Called before <see cref="Services"/> is built.
    /// Can be used by UI tests to override services.
    /// </summary>
    internal static event EventHandler<IServiceCollection>? BeforeBuildServiceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Remove DataAnnotations validation plugin since we're using INotifyDataErrorInfo from MvvmToolkit
        var dataValidationPluginsToRemove = BindingPlugins
            .DataValidators.OfType<DataAnnotationsValidationPlugin>()
            .ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }

        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode)
        {

        }
        else
        {
            ConfigureServiceProvider();
        }

        if (DesktopLifetime is not null)
        {
            DesktopLifetime.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Setup();

            ShowMainWindow();
        }
    }

    private void SetFontFamily(FontFamily fontFamily)
    {

    }

    //public FontFamily GetPlatformDefaultFontFamily()
    //{

    //}


    private void Setup()
    {

    }

    private void ShowMainWindow()
    {
        if (DesktopLifetime is null)
            return;

        var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = mainViewModel;

        mainWindow.ExtendClientAreaChromeHints = Program.Args.NoWindowChromeEffects
            ? ExtendClientAreaChromeHints.NoChrome
            : ExtendClientAreaChromeHints.PreferSystemChrome;

        var settingsManager = Services.GetRequiredService<ISettingsManager>();
        var windowSettings = settingsManager.Settings.WindowSettings;
        if (windowSettings != null && !Program.Args.ResetWindowPosition)
        {
            mainWindow.Position = new PixelPoint(windowSettings.X, windowSettings.Y);
            mainWindow.Width = windowSettings.Width;
            mainWindow.Height = windowSettings.Height;
            mainWindow.WindowState = windowSettings.IsMaximized ? WindowState.Maximized : WindowState.Normal;
        }
        else
        {
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        VisualRoot = mainWindow;
        StorageProvider = mainWindow.StorageProvider;
        Clipboard = mainWindow.Clipboard ?? throw new NullReferenceException("Clipboard is null");

        DesktopLifetime.MainWindow = mainWindow;
        DesktopLifetime.Exit += OnApplicationLifetimeExit;
        DesktopLifetime.ShutdownRequested += OnShutdownRequested;

        AppDomain.CurrentDomain.ProcessExit += OnExit;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        // Since we're manually shutting down NLog in OnExit
        LogManager.AutoShutdown = false;
    }

    private static void ConfigureServiceProvider()
    {
        var services = ConfigureServices();

        BeforeBuildServiceProvider?.Invoke(null, services);

        Services = services.BuildServiceProvider();

        var settingsManager = Services.GetRequiredService<ISettingsManager>();


    }

    internal static void ConfigurePageViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
    }

    internal static void ConfigureDialogViewModels(IServiceCollection services, Type[] exportedTypes)
    {

    }

    internal static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();



        var exportedTypes = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("SmartGenealogy") == true)
            .SelectMany(a => a.GetExportedTypes())
            .ToArray();

        var transientTypes = exportedTypes
            .Select(t => new { t, attributes = t.GetCustomAttributes(typeof(TransientAttribute), false) })
            .Where(
                t1 =>
                    t1.attributes is { Length: > 0 }
                    && !t1.t.Name.Contains("Mock", StringComparison.OrdinalIgnoreCase))
            .Select(t1 => new { Type = t1.t, Attribute = (TransientAttribute)t1.attributes[0] });

        foreach (var typePair in transientTypes)
        {
            if (typePair.Attribute.InterfaceType is null)
            {
                services.AddTransient(typePair.Type);
            }
            else
            {
                services.AddTransient(typePair.Attribute.InterfaceType, typePair.Type);
            }
        }

        var singletonTypes = exportedTypes
            .Select(t => new { t, attributes = t.GetCustomAttributes(typeof(SingletonAttribute), false) })
            .Where(
                t1 =>
                    t1.attributes is { Length: > 0 }
                    && !t1.t.Name.Contains("Mock", StringComparison.OrdinalIgnoreCase))
            .Select(
                t1 => new { Type = t1.t, Attributes = t1.attributes.Cast<SingletonAttribute>().ToArray() });

        foreach (var typePair in singletonTypes)
        {
            foreach (var attribute in typePair.Attributes)
            {
                if (attribute.InterfaceType is null)
                {
                    services.AddSingleton(typePair.Type);
                }
                else if (attribute.ImplType is not null)
                {
                    services.AddSingleton(attribute.InterfaceType, attribute.ImplType);
                }
                else
                {
                    services.AddSingleton(attribute.InterfaceType, typePair.Type);
                }

                // IDisposable registering
                var serviceType = attribute.InterfaceType ?? typePair.Type;

                if (serviceType == typeof(IDisposable) || serviceType == typeof(IAsyncDisposable))
                {
                    continue;
                }

                if (typePair.Type.IsAssignableTo(typeof(IDisposable)))
                {
                    Debug.WriteLine("Registering IDisposable: {Name}", typePair.Type.Name);
                    services.AddSingleton<IDisposable>(
                        provider => (IDisposable)provider.GetRequiredService(serviceType));
                }

                if (typePair.Type.IsAssignableTo(typeof(IAsyncDisposable)))
                {
                    Debug.WriteLine("Registering IAsyncDisposable: {Name}", typePair.Type.Name);
                    services.AddSingleton<IAsyncDisposable>(
                        provider => (IAsyncDisposable)provider.GetRequiredService(serviceType));
                }
            }
        }

        ConfigurePageViewModels(services);
        ConfigureDialogViewModels(services, exportedTypes);



        var logConfig = ConfigureLogging();

        // Add logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder
                .AddFilter("Microsoft.Extensions.Http", LogLevel.Warning)
                .AddFilter("Microsoft.Extensions.Http.DefaultHttpClientFactory", LogLevel.Warning)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            builder.SetMinimumLevel(LogLevel.Trace);
#if DEBUG
            builder.AddNLog(
                logConfig,
                new NLogProviderOptions
                {
                    IgnoreEmptyEventId = false,
                    CaptureEventId = EventIdCaptureType.Legacy
                }
            );
#else
            builder.AddNLog(logConfig);
#endif
        });

        return services;
    }

    /// <summary>
    /// Requests shutdown of the Current Application.
    /// </summary>
    /// <remarks>This returns asynchronously *without waiting* for Shutdown</remarks>
    /// <param name="exitCode">Exit code for the application.</param>
    /// <exception cref="NullReferenceException">If Application.Current is null</exception>
    public static void Shutdown(int exitCode = 0)
    {
        if (Current is null)
            throw new NullReferenceException("Current Application was null when Shutdown called");

        if (Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            try
            {
                var result = lifetime.TryShutdown(exitCode);
                Debug.WriteLine($"Shutdown: {result}");

                if (result)
                {
                    Environment.Exit(exitCode);
                }
            }
            catch (InvalidOperationException)
            {
                // Ignore in case already shutting down
            }
        }
        else
        {
            Environment.Exit(exitCode);
        }
    }

    private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        Debug.WriteLine("Start OnShutdownRequested");

        if (e.Cancel)
            return;

        // Check if we need to dispose IAsyncDisposables
        if (IsAsyncDisposeComplete
            || Services.GetServices<IAsyncDisposable>().ToList() is not { Count: > 0 } asyncDisposables)
            return;

        // Cancel shutdown for now
        e.Cancel = true;
        IsAsyncDisposeComplete = true;

        Debug.WriteLine("OnShutdownRequested Canceled: Disposing IAsyncDisposables");

        Dispatcher
            .UIThread.InvokeAsync(async () =>
            {
                foreach (var disposable in asyncDisposables)
                {
                    Debug.WriteLine($"Disposing IAsyncDisposable ({disposable.GetType().Name})");
                    try
                    {
                        await disposable.DisposeAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                }
            })
            .ContinueWith(_ =>
            {
                // Shutdown again
                Debug.WriteLine("Finished disposing IAsyncDisposables, shutting down");

                if (Dispatcher.UIThread.SupportsRunLoops)
                {
                    Dispatcher.UIThread.Invoke(() => Shutdown());
                }

                Environment.Exit(0);
            })
            .SafeFireAndForget();
    }

    private void OnApplicationLifetimeExit(object? sender, ControlledApplicationLifetimeExitEventArgs args)
    {
        Logger.Debug("OnApplicationLifetimeExit: {@Args}", args);

        OnExit(sender, args);
    }

    private void OnExit(object? sender, EventArgs _)
    {

    }

    private static void TaskScheduler_UnobservedTaskException(
        object? sender,
        UnobservedTaskExceptionEventArgs e)
    {
        if (e.Exception is not Exception unobservedEx)
            return;

        try
        {
            //var notificationService = Services.GetRequiredService<INotificationService>();

            //Dispatcher.UIThread.Invoke(() =>
            //{
            //    var originException = unobservedEx.InnerException ?? unobservedEx;
            //    notificationService.ShowPersistent(
            //        $"Unobserved Task Exception - {originException.GetType().Name}",
            //        originException.Message);
            //});

            //// Consider the exception observed if we were able to show a notification
            //e.SetObserved();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to show Unobserved Task Exception notification");
        }
    }

    private static async void OnActivated(object? sender, ActivatedEventArgs args)
    {

    }

    private static LoggingConfiguration ConfigureLogging()
    {
        var setupBuilder = LogManager.Setup();



        return LogManager.Configuration;
    }
}