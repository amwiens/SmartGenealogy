﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using AsyncAwaitBestPractices;

using AsyncImageLoader;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

using CommandLine;

using NLog;

using Polly.Contrib.WaitAndRetry;

using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

using Semver;

using Sentry;

using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Processes;
using SmartGenealogy.Core.Updater;
using SmartGenealogy.Models;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy;

public static class Program
{
    private static Logger? _logger;
    private static Logger Logger => _logger ??= LogManager.GetCurrentClassLogger();

    public static AppArgs Args { get; private set; } = new();

    public static bool IsDebugBuild { get; private set; }

    public static Stopwatch StartupTimer { get; } = new();

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        StartupTimer.Start();

        SetDebugBuild();

        var parseResult = Parser
            .Default.ParseArguments<AppArgs>(args)
            .WithNotParsed(errors =>
            {
                foreach (var error in errors)
                {
                    Console.Error.WriteLine(error.ToString());
                }
            });

        if (parseResult.Errors.Any(
            x => x.Tag is ErrorType.HelpRequestedError or ErrorType.VersionRequestedError))
        {
            Environment.Exit(0);
            return;
        }

        Args = parseResult.Value ?? new AppArgs();

        if (Args.HomeDirectoryOverride is { } homeDir)
        {
            Compat.SetAppDataHome(homeDir);
            GlobalConfig.HomeDir = homeDir;
        }



        if (Args.WaitForExitPid is { } waitExitPid)
        {
            WaitForPidExit(waitExitPid, TimeSpan.FromSeconds(30));
        }

        HandleUpdateReplacement();
        HandleUpdateCleanup();

        var infoVersion = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;
        Compat.AppVersion = SemVersion.Parse(infoVersion ?? "0.0.0", SemVersionStyles.Strict);

        // Configure exception dialog for unhandled exceptions
        if (!Debugger.IsAttached || Args.DebugExceptionDialog)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        // Configure Sentry
        if (!Args.NoSentry && (!Debugger.IsAttached || Args.DebugSentry))
        {
            ConfigureSentry();
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }



    /// <summary>
    /// Called in <see cref="BuildAvaloniaApp"/> and UI tests to setup static configurations
    /// </summary>
    internal static void SetupAvaloniaApp()
    {
        IconProvider.Current.Register<FontAwesomeIconProvider>();

        // Use our custom image loader for custom local load error handling
        ImageLoader.AsyncImageLoader.Dispose();
        ImageLoader.AsyncImageLoader = new FallbackRamCachedWebImageLoader();


    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        SetupAvaloniaApp();

        var app = AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();



        return app;
    }

    private static void HandleUpdateReplacement()
    {
        // Check if we're in the named update folder
        if (Compat.AppCurrentDir.Name is not (UpdateHelper.UpdateFolderName or "Update"))
            return;

        if (Compat.AppCurrentDir.Parent is not { } parentDir)
            return;

        // Copy our current file to the parent directory, overwriting the old app file

        var isCopied = false;

        foreach (
            var delay in Backoff.DecorrelatedJitterBackoffV2(
                TimeSpan.FromMilliseconds(300),
                retryCount: 6,
                fastFirst: true))
        {
            try
            {
                if (Compat.IsMacOS)
                {
                    var currentApp = Compat.AppBundleCurrentPath!;
                    var targetApp = parentDir.JoinDir(Compat.GetAppName());

                    // Since macOS has issues with signature caching, delete the target app first
                    if (targetApp.Exists)
                    {
                        targetApp.Delete(true);
                    }

                    currentApp.CopyTo(targetApp);
                }
                else
                {
                    var currentExe = Compat.AppCurrentPath;
                    var targetExe = parentDir.JoinFile(Compat.GetExecutableName());

                    currentExe.CopyTo(targetExe, true);
                }

                isCopied = true;
                break;
            }
            catch (Exception)
            {
                Thread.Sleep(delay);
            }
        }

        if (!isCopied)
        {
            Logger.Error("Failed to copy current executable to parent directory");
            Environment.Exit(1);
        }

        var targetAppOrBundle = Path.Combine(parentDir, Compat.GetAppName());

        // Ensure permissions are set for unix
        if (Compat.IsUnix)
        {
            File.SetUnixFileMode(
                targetAppOrBundle, // 0755
                UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupExecute
                    | UnixFileMode.OtherRead
                    | UnixFileMode.OtherExecute);
        }

        // Start the new app while passing our own PID to wait for exit
        ProcessRunner.StartApp(
            targetAppOrBundle,
            new[] { "--wait-for-exit-pid", $"{Environment.ProcessId}" });

        // Shutdown the current app
        Environment.Exit(0);
    }

    private static void HandleUpdateCleanup()
    {
        // Delete update folder if it exists in current directory
        if (UpdateHelper.UpdateFolder is { Exists: true } updateDir)
        {
            try
            {
                updateDir.Delete(true);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to delete update folder");
            }
        }
    }

    /// <summary>
    /// Wait for an external process to exit,
    /// ignores if process is not found, already exited, or throws an exception
    /// </summary>
    /// <param name="pid">External process PID</param>
    /// <param name="timeout">Timeout to wait for process to exit</param>
    private static void WaitForPidExit(int pid, TimeSpan timeout)
    {
        try
        {
            var process = Process.GetProcessById(pid);
            process.WaitForExitAsync(new CancellationTokenSource(timeout).Token).GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
            Logger.Warn("Timed out ({Timeout:g}) waiting for process {Pid} to exit", timeout, pid);
        }
        catch (SystemException e)
        {
            Logger.Warn(e, "Failed to wait for process {Pid} to exit", pid);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Unexpected error during WaitForPidExit");
            throw;
        }
    }

    private static void ConfigureSentry()
    {
        SentrySdk.Init(o =>
        {
            o.Dsn =
                "https://8926beb0544eced376af258e663e0500@o4508373692186624.ingest.us.sentry.io/4508373697560581";
            o.StackTraceMode = StackTraceMode.Enhanced;
            o.TracesSampleRate = 1.0;
            o.IsGlobalModeEnabled = true;
            // Enables Sentry's "Release Health" feature.
            o.AutoSessionTracking = true;
            // 1.0 to capture 100% of transactions for performance monitoring.
            o.TracesSampleRate = 1.0;
#if DEBUG
            o.Environment = "Development";
#endif
            // Filters
            o.SetBeforeSend(
                (sentryEvent, _) =>
                {
                    // Ignore websocket errors from ComfyClient
                    if (sentryEvent.Logger == "Websocket.Client.WebsocketClient")
                    {
                        return null;
                    }

                    return sentryEvent;
                });
        });
    }

    private static void TaskScheduler_UnobservedTaskException(
        object? sender,
        UnobservedTaskExceptionEventArgs e)
    {
        if (e.Exception is Exception ex)
        {
            Logger.Error(ex, "Unobserved Task Exception");
        }
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is not Exception ex)
            return;

        SentryId? sentryId = null;

        // Exception automatically logged by Sentry if enabled
        if (SentrySdk.IsEnabled)
        {
            ex.SetSentryMechanism("AppDomain.UnhandledException", handled: false);
            sentryId = SentrySdk.CaptureException(ex);
            SentrySdk.FlushAsync().SafeFireAndForget();
        }
        else
        {
            Logger.Fatal(ex, "Unhandled {Type}: {Message}", ex.GetType().Name, ex.Message);
        }

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var dialog = new ExceptionDialog
            {
                DataContext = new ExceptionViewModel { Exception = ex, SentryId = sentryId }
            };

            var mainWindow = lifetime.MainWindow;
            // We can only show dialog if main window exists, and is visible
            if (mainWindow is { PlatformImpl: not null, IsVisible: true })
            {
                // Configure for dialog mode
                dialog.ShowAsDialog = true;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                // Show synchronously without blocking UI thread
                // https://github.com/AvaloniaUI/Avalonia/issues/4810#issuecomment-704259221
                var cts = new CancellationTokenSource();

                dialog
                    .ShowDialog(mainWindow)
                    .ContinueWith(
                        _ =>
                        {
                            cts.Cancel();
                            ExitWithException(ex);
                        },
                        TaskScheduler.FromCurrentSynchronizationContext());

                Dispatcher.UIThread.MainLoop(cts.Token);
            }
            else
            {
                // No parent window available
                var cts = new CancellationTokenSource();
                // Exit on token cancellation
                cts.Token.Register(() => ExitWithException(ex));

                dialog.ShowWithCts(cts);

                Dispatcher.UIThread.MainLoop(cts.Token);
            }
        }
    }

    [DoesNotReturn]
    private static void ExitWithException(Exception exception)
    {
        if (SentrySdk.IsEnabled)
        {
            SentrySdk.Flush();
        }
        App.Shutdown(1);
        Dispatcher.UIThread.InvokeShutdown();
        Environment.Exit(Marshal.GetHRForException(exception));
    }

    [Conditional("DEBUG")]
    private static void SetDebugBuild()
    {
        IsDebugBuild = true;
    }
}