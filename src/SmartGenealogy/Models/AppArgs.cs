using CommandLine;

namespace SmartGenealogy.Models;

/// <summary>
/// Command line arguments passed to the application.
/// </summary>
public class AppArgs
{
    /// <summary>
    /// Whether to enable debug mode
    /// </summary>
    [Option("debug", HelpText = "Enable debug mode")]
    public bool DebugMode { get; set; }

    /// <summary>
    /// Whether to use the exception dialog while debugger is attached.
    /// When no debugger is attached, the exception dialog is always used.
    /// </summary>
    [Option("debug-exception-dialog", HelpText = "Use exception dialog while debugger is attached")]
    public bool DebugExceptionDialog { get; set; }



    /// <summary>
    /// Whether to disable window chrome effects
    /// </summary>
    [Option("no-window-chrome-effects", HelpText = "Disable window chrome effects")]
    public bool NoWindowChromeEffects { get; set; }

    /// <summary>
    /// Flag to indicate if we should reset the saved window position back to (0,0)
    /// </summary>
    [Option("reset-window-position", HelpText = "Reset the saved window position back to (0,0)")]
    public bool ResetWindowPosition { get; set; }
}