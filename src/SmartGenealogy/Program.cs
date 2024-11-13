using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Threading;

using CommandLine;

using NLog;

using SmartGenealogy.Models;

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


        var infoVersion = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;


        // Configure exception dialog for unhandled exceptions
        if (!Debugger.IsAttached || Args.DebugExceptionDialog)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var app = AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();



        return app;
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


    }

    [DoesNotReturn]
    private static void ExitWithException(Exception exception)
    {
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