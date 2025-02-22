using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Input;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Processes;
using SmartGenealogy.Languages;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels.Dialogs;

[View(typeof(ExceptionDialog))]
[ManagedService]
[RegisterTransient<ExceptionViewModel>()]
public partial class ExceptionViewModel : ViewModelBase
{
    public Exception? Exception { get; set; }

    public bool IsRecoverable { get; set; }

    public string Description =>
        IsRecoverable
            ? Resources.Text_UnexpectedErrorRecoverable_Description
            : Resources.Text_UnexpectedError_Description;

    public string? Message => Exception?.Message;

    public string? ExceptionType => Exception?.GetType().Name ?? "";

    public bool IsContinueResult { get; set; }

    public string? LogZipPath { get; set; }

    public static async Task<string> CreateLogFolderZip()
    {
        var tcs = new TaskCompletionSource();
        LogManager.Flush(
            ex =>
            {
                if (ex is null)
                {
                    tcs.SetResult();
                }
                else
                {
                    tcs.SetException(ex);
                }
            },
            TimeSpan.FromSeconds(15));
        await tcs.Task;

        using var suspend = LogManager.SuspendLogging();

        var logDir = Compat.AppDataHome.JoinDir("Logs");

        // Copy logs to temp directory
        using var tempDir = new TempDirectoryPath();
        var tempLogDir = tempDir.JoinDir("Logs");
        tempLogDir.Create();
        foreach (var logFile in logDir.EnumerateFiles("*.log"))
        {
            // Need FileShare.ReadWrite since NLog keeps the file open
            await logFile.CopyToAsync(
                tempLogDir.JoinFile(logFile.Name),
                FileShare.ReadWrite,
                overwrite: true);
        }

        // Find a unique name for the output archive
        var archiveNameBase = $"smartgenealogy-log-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";
        var archiveName = archiveNameBase;
        var archivePath = Compat.AppDataHome.JoinFile(archiveName + ".zip");
        var i = 1;
        while (File.Exists(archivePath))
        {
            archiveName = $"{archiveNameBase}-{i++}";
            archivePath = Compat.AppDataHome.JoinFile(archiveName + ".zip");
        }

        // Create the archive
        ZipFile.CreateFromDirectory(tempLogDir, archivePath, CompressionLevel.Optimal, false);

        return archivePath;
    }

    [RelayCommand]
    private async Task OpenLogZipInFileBrowser()
    {
        if (string.IsNullOrWhiteSpace(LogZipPath) || !File.Exists(LogZipPath))
        {
            LogZipPath = await CreateLogFolderZip();
        }

        await ProcessRunner.OpenFileBrowser(LogZipPath);
    }

    [Localizable(false)]
    public string? FormatAsMarkdown()
    {
        var msgBuilder = new StringBuilder();
        msgBuilder.AppendLine();

        if (Exception is not null)
        {
            msgBuilder.AppendLine("## Exception");
            msgBuilder.AppendLine($"```{ExceptionType}: {Message}```");

            if (Exception.InnerException is not null)
            {
                msgBuilder.AppendLine(
                    $"```{Exception.InnerException.GetType().Name}: {Exception.InnerException.Message}```");
            }
        }
        else
        {
            msgBuilder.AppendLine("## Exception");
            msgBuilder.AppendLine("```(None)```");
        }

        if (Exception?.StackTrace is not null)
        {
            msgBuilder.AppendLine("### Stack Trace");
            msgBuilder.AppendLine("```{Exception.StackTrace}```");
        }

        if (Exception?.InnerException is { StackTrace: not null } innerException)
        {
            msgBuilder.AppendLine($"```{innerException.StackTrace}```");
        }

        return msgBuilder.ToString();
    }

    [Localizable(false)]
    private static string GetIssueUrl()
    {
        return $"";
    }
}