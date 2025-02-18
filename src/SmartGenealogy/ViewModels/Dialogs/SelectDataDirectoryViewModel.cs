using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels.Dialogs;

[View(typeof(SelectDataDirectoryDialog))]
[ManagedService]
[RegisterTransient<SelectDataDirectoryViewModel>]
public partial class SelectDataDirectoryViewModel : ContentDialogViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static string DefaultInstallLocation =>
        Compat.IsLinux
            ? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "SmartGenealogy")
            : Compat.AppDataHome;

    private readonly ISettingsManager settingsManager;

    private const string ValidExistingDirectoryText = "Valid existing data directory found";
    private const string InvalidDirectoryText = "Directory must be empty or have a valid settings.json file";
    private const string NotEnoughFreeSpaceText = "Not enough free space on the selected drive";
    private const string FatWarningText = "FAT32 / exFAT drives are not supported at this time";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsInTempFolder))]
    private string dataDirectory = DefaultInstallLocation;

    [ObservableProperty]
    private bool isPortableMode = Compat.IsLinux;

    [ObservableProperty]
    private string directoryStatusText = string.Empty;

    [ObservableProperty]
    private bool isStatusBadgeVisible;

    [ObservableProperty]
    private bool isDirectoryValid;

    [ObservableProperty]
    private bool showFatWarning;

    public bool IsInTempFolder =>
        Compat
            .AppCurrentDir.ToString()
            .StartsWith(
                Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar),
                StringComparison.OrdinalIgnoreCase);



    public SelectDataDirectoryViewModel(ISettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;

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