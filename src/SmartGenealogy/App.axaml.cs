using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
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
using Avalonia.Platform.Storage;
using Avalonia.Threading;

using FluentAvalonia.Interop;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Config;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Extensions;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Helpers;
using SmartGenealogy.Languages;
using SmartGenealogy.ViewModels;
using SmartGenealogy.Views;

namespace SmartGenealogy;

/// <summary>
/// Main application class for the Smart Genealogy App.
/// </summary>
public sealed class App : Application
{
    private static readonly Lazy<Logger> LoggerLazy = new(LogManager.GetCurrentClassLogger);
    private static Logger Logger => LoggerLazy.Value;

    private readonly SemaphoreSlim onExitSemaphore = new(1, 1);

    /// <summary>
    /// True if <see cref="OnShutdownRequested"/> has started async dispose of services.
    /// </summary>
    private bool isAsyncDisposeStarted;

    /// <summary>
    /// True if <see cref="OnShutdownRequested"/> has completed async dispose of services.
    /// </summary>
    private bool isAsyncDisposeComplete;

    private bool isOnExitComplete;

    private ServiceProvider? serviceProvider;

    [NotNull]
    public static Visual? VisualRoot { get; internal set; }

    public static TopLevel TopLevel => TopLevel.GetTopLevel(VisualRoot).Unwrap();

    public static IStorageProvider StorageProvider => TopLevel.StorageProvider;

    public static IClipboard? Clipboard => TopLevel.Clipboard;

    public IClassicDesktopStyleApplicationLifetime? DesktopLifetime =>
        ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

    public new static App? Current => (App?)Application.Current;

    [NotNull]
    public static IServiceProvider? Services =>
        Design.IsDesignMode ? DesignData.DesignData.Services : Current?.serviceProvider;

    internal static bool IsHeadlessMode =>
        TopLevel.TryGetPlatformHandle()?.HandleDescriptor is null or "STUB";

    /// <summary>
    /// Called before <see cref="Services"/> is built. Can be used by UI tests to override services.
    /// </summary>
    internal static event EventHandler<IServiceCollection>? BeforeBuildServiceProvider;

    /// <summary>
    /// Initializes the application by loading XAML resources.
    /// </summary>
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
            DesignData.DesignData.Initialize();
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

    /// <summary>
    /// Set the default font family for the application.
    /// </summary>
    private void SetFontFamily(FontFamily fontFamily)
    {
        Resources["ContentControlThemeFontFamily"] = fontFamily;
    }

    /// <summary>
    /// Get the default font family for the current platform and language.
    /// </summary>
    public FontFamily GetPlatformDefaultFontFamily()
    {
        try
        {
            var fonts = new List<string>();

            if (Cultures.Current?.Name == "ja-JP")
            {
                return Resources["NotoSansJP"] as FontFamily
                    ?? throw new ApplicationException("Font NotoSansJP not found");
            }

            if (Compat.IsWindows)
            {
                fonts.Add(OSVersionHelper.IsWindows11() ? "Segoe UI Variable TExt" : "Segoe UI");
            }
            else if (Compat.IsMacOS)
            {
                // Use Segoe fonts if installed, but we can't distrubute them
                fonts.Add("Segoe UI Variable");
                fonts.Add("Segoe UI");

                fonts.Add("San Francisco");
                fonts.Add("Helvetica Neue");
                fonts.Add("Helvetica");
            }
            else
            {
                return FontFamily.Default;
            }

            return new FontFamily(string.Join(",", fonts));
        }
        catch (Exception ex)
        {
            Logger.Error(ex);

            return FontFamily.Default;
        }
    }

    /// <summary>
    /// Setup tasks to be run shortly before any window is shown
    /// </summary>
    private void Setup()
    {
        //using var _ = CodeTimer.StartNew();

        ////// Setup uri handler for `smartgenealogy://` protocol
        ////Program.UriHandler.RegisterUriScheme();

        //// Setup activation protocol handlers (uri handler on macOS)
        //if (Compat.IsMacOS && this.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
        //{
        //    Logger.Debug("ActivatableLifetime available, setting up activation protocol handlers");
        //    activatableLifetime.Activated += OnActivated;
        //}
    }

    private void ShowMainWindow()
    {
        if (DesktopLifetime is null)
            return;

        var mainWindow = Services.GetRequiredService<MainWindow>();
        VisualRoot = mainWindow;

        DesktopLifetime.MainWindow = mainWindow;
        DesktopLifetime.Exit += OnApplicationLifetimeExit;
        DesktopLifetime.ShutdownRequested += OnShutdownRequested;

        AppDomain.CurrentDomain.ProcessExit += OnExit;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        // Since we're manually shutting down NLog in OnExit
        LogManager.AutoShutdown = false;
    }

    [MemberNotNull(nameof(serviceProvider))]
    private void ConfigureServiceProvider()
    {
        var services = ConfigureServices();

        BeforeBuildServiceProvider?.Invoke(null, services);

        serviceProvider = services.BuildServiceProvider();
    }

    internal static void ConfigurePageViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>(
            provider =>
                new MainWindowViewModel()
                {
                    Pages =
                    {
                        provider.GetRequiredService<HomeViewModel>(),
                        provider.GetRequiredService<OllamaViewModel>(),
                    },
                    FooterPages =
                    {
                        provider.GetRequiredService<SettingsViewModel>(),
                    }
                });
    }

    internal static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        //services.AddMemoryCache();
        services.AddLazyInstance();

        // Register services by attributes
        services.AddServicesByAttributes();

        ConfigurePageViewModels(services);

        services.AddServiceManagerWithCurrentCollectionServices<ViewModelBase>(
            s => s.ServiceType.GetCustomAttributes<ManagedServiceAttribute>().Any());

        var logConfig = ConfigureLogging();

        // add logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder
                .AddFilter("Microsoft.Extensions.Http", Microsoft.Extensions.Logging.LogLevel.Warning)
                .AddFilter("Microsoft.Extensions.Http.DefaultHttpClientFactory", Microsoft.Extensions.Logging.LogLevel.Warning)
                .AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning)
                .AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning);
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
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
        Logger.Trace("Start OnShutdownRequested");

        if (e.Cancel)
            return;

        // Skip if Async Dispose already started, shutdown will be handled by it
        if (isAsyncDisposeStarted)
            return;

        // Cancel shutdown for now to dispose
        e.Cancel = true;
        isAsyncDisposeStarted = true;

        Logger.Trace("OnShutdownRequested Canceled: Disposing IAsyncDisposables");

        Dispatcher
            .UIThread.InvokeAsync(async () =>
            {
                if (serviceProvider is null)
                {
                    Logger.Warn("Service Provider is null, skipping Async Dispose");
                    return;
                }

                var settingsManager = Services.GetRequiredService<ISettingsManager>();

                Logger.Debug("Disposing App Services");
                try
                {
                    OnServiceProviderDisposing(serviceProvider);
                    await serviceProvider.DisposeAsync();
                    isAsyncDisposeComplete = true;
                }
                catch (Exception disposeEx)
                {
                    Logger.Error(disposeEx, "Failed to dispose ServiceProvider");
                }

                Logger.Debug("Flushing SettingsManager");
                try
                {
                    var cts = new CancellationTokenSource(5000);
                    //await settingsManager.FlushAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger.Error("Timeout Flushing SettingsManager");
                }
            })
            .ContinueWith(_ =>
            {
                // Shutdown again
                Logger.Debug("Finished async shutdown tasks, shutting down");

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
        // Skip if already run
        if (isOnExitComplete)
        {
            return;
        }

        // Skip if another OnExit is running
        if (!onExitSemaphore.Wait(0))
        {
            // Block until the other OnExit is done to delay shutdown
            onExitSemaphore.Wait();
            onExitSemaphore.Release();
            return;
        }

        try
        {
            if (serviceProvider is null)
            {
                Logger.Warn("Service Provider is null, skipping OnExit");
                return;
            }

            // Dispose services only if async dispose has not completed
            if (!isAsyncDisposeComplete)
            {
                Logger.Debug("OnExit: Disposing App Services");

                OnServiceProviderDisposing(serviceProvider);
                serviceProvider.Dispose();
            }

            Logger.Debug("OnExit: Finished");
        }
        finally
        {
            isOnExitComplete = true;
            onExitSemaphore.Release();

            LogManager.Shutdown();
        }
    }

    private static void OnServiceProviderDisposing(ServiceProvider serviceProvider)
    {
        //var disposables = serviceProvider.GetDisposables();
        //disposables.RemoveAll(d => d is NamedPipeWorker);

        //Logger.Trace("Disposing {Count} Disposables", disposables.Count);
    }

    private static void TaskScheduler_UnobservedTaskException(
        object? sender,
        UnobservedTaskExceptionEventArgs e)
    {
    }

    private static LoggingConfiguration ConfigureLogging()
    {
        var setupBuilder = LogManager.Setup();

        LogManager.ReconfigExistingLoggers();

        return LogManager.Configuration;
    }
}