using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AsyncAwaitBestPractices;

using Avalonia.Platform.Storage;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models.Progress;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels.Dialogs;

[View(typeof(CreateProjectDialog))]
[ManagedService]
[Transient]
public partial class CreateProjectViewModel : ContentDialogViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static string DefaultInstallLocation =>
        Environment.GetFolderPath(Environment.SpecialFolder.Personal);

    private readonly IProjectSettingsManager projectSettingsManager;

    private const string ValidExistingDirectoryText = "Valid existing data directory found";
    private const string InvalidDirectoryText = "Directory must be empty of having a valid sgproj file";

    [ObservableProperty]
    private string projectName;

    [ObservableProperty]
    private string dataDirectory = DefaultInstallLocation;

    [ObservableProperty]
    private string directoryStatusText = string.Empty;

    [ObservableProperty]
    private bool isStatusBadgeVisible;

    [ObservableProperty]
    private bool isDirectoryValid;

    public RefreshBadgeViewModel ValidatorRefreshBadge { get; } =
        new()
        {
            State = ProgressState.Inactive,
            SuccessToolTipText = ValidExistingDirectoryText,
            FailToolTipText = InvalidDirectoryText
        };

    public CreateProjectViewModel(IProjectSettingsManager projectSettingsManager)
    {
        this.projectSettingsManager = projectSettingsManager;
        ValidatorRefreshBadge.RefreshFunc = ValidateDataDirectory;
    }

    public override void OnLoaded()
    {
        ValidatorRefreshBadge.RefreshCommand.ExecuteAsync(null).SafeFireAndForget();
    }

    // Revalidate on data directory change
    partial void OnDataDirectoryChanged(string value)
    {
        ValidatorRefreshBadge.RefreshCommand.ExecuteAsync(null).SafeFireAndForget();
    }

    private async Task<bool> ValidateDataDirectory()
    {
        await using var delay = new MinimumDelay(100, 200);

        // Doesn't exist, this is fine as a new install, hide badge
        if (!Directory.Exists(DataDirectory))
        {
            IsStatusBadgeVisible = false;
            IsDirectoryValid = true;
            return true;
        }
        // Otherwise check that a sgproj exists
        var settingsPath = Path.Combine(DataDirectory, $"{ProjectName}.sgproj");

        // sgproj exists: Try deserializing it
        if (File.Exists(settingsPath))
        {
            try
            {
                var jsonText = await File.ReadAllTextAsync(settingsPath);
                var _ = JsonSerializer.Deserialize<Core.Models.Settings.ProjectSettings>(
                    jsonText,
                    new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                // If successful, show existing badge
                IsStatusBadgeVisible = true;
                IsDirectoryValid = true;
                DirectoryStatusText = ValidExistingDirectoryText;
                return true;
            }
            catch (Exception e)
            {
                Logger.Info("Failed to deserialize sgproj: {Msg}", e.Message);
                // If not, show error badge, and set directory to invalid to prevent continuing
                IsStatusBadgeVisible = true;
                IsDirectoryValid = false;
                DirectoryStatusText = InvalidDirectoryText;
                return false;
            }
        }

        // No sgproj file

        // Check if the directory is empty: hide badge and set directory to valid
        var isEmpty = !Directory.EnumerateFileSystemEntries(DataDirectory).Any();
        if (isEmpty)
        {
            IsStatusBadgeVisible = false;
            IsDirectoryValid = true;
            return true;
        }

        // Not empty: show error badge, and set directory to invalid
        IsStatusBadgeVisible = true;
        IsDirectoryValid = false;
        DirectoryStatusText = InvalidDirectoryText;
        return false;
    }

    private bool CanPickFolder => App.StorageProvider.CanPickFolder;

    [RelayCommand(CanExecute = nameof(CanPickFolder))]
    private async Task ShowFolderBrowserDialog()
    {
        var provider = App.StorageProvider;
        var result = await provider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { Title = "Select Data Folder", AllowMultiple = false });

        if (result.Count != 1)
            return;

        DataDirectory = result[0].Path.LocalPath;
    }
}