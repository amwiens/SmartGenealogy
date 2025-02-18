using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using FluentAvalonia.UI.Controls;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Languages;
using SmartGenealogy.Messages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IRecipient<FileOpenedMessage>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ISettingsManager settingsManager;
    private readonly ServiceManager<ViewModelBase> dialogFactory;

    private readonly INotificationService notificationService;

    [ObservableProperty]
    private PageViewModelBase? currentPage;

    [ObservableProperty]
    private object? _selectedCategory;

    [ObservableProperty]
    public List<PageViewModelBase> pages = new();

    [ObservableProperty]
    public List<PageViewModelBase> footerPages = new();

    public double PaneWidth =>
        Cultures.Current switch
        {
            _ => 200
        };

    public MainWindowViewModel(
        ISettingsManager settingsManager,
        ServiceManager<ViewModelBase> dialogFactory,
        INotificationService notificationService)
    {
        this.settingsManager = settingsManager;
        this.dialogFactory = dialogFactory;

        this.notificationService = notificationService;

        WeakReferenceMessenger.Default.Register<FileOpenedMessage>(this);
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        // Set only if null, since this may be called again when content dialogs open
        CurrentPage ??= Pages.FirstOrDefault();
        SelectedCategory ??= Pages.FirstOrDefault();
    }

    protected override async Task OnInitialLoadedAsync()
    {
        await base.OnInitialLoadedAsync();

        // Skip if design mode
        if (Design.IsDesignMode)
            return;

        if (!await EnsureDataDirectory())
        {
            // False if user exited dialog, shutdown app
            App.Shutdown();
            return;
        }
    }



    /// <summary>
    /// Check if the data directory exists, if not, show the select data directory dialog.
    /// </summary>
    private async Task<bool> EnsureDataDirectory()
    {
        // If we can't find library, show selection dialog
        var foundInitially = settingsManager.TryFindLibrary();
        if (!foundInitially)
        {
            var result = await ShowSelectDataDirectoryDialog();
            if (!result)
                return false;
        }

        // Try to find library again, should be found now
        if (!settingsManager.TryFindLibrary())
        {
            throw new System.Exception("Could not find library after settings path");
        }

        return true;
    }



    /// <summary>
    /// Shows the select data directory dialog.
    /// </summary>
    /// <returns>true if path set successfully, false if user exited dialog.</returns>
    private async Task<bool> ShowSelectDataDirectoryDialog()
    {
        var viewModel = dialogFactory.Get<SelectDataDirectoryViewModel>();
        var dialog = new BetterContentDialog
        {
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            IsFooterVisible = false,
            Content = new SelectDataDirectoryDialog { DataContext = viewModel }
        };

        var result = await dialog.ShowAsync(App.TopLevel);
        if (result == ContentDialogResult.Primary)
        {
            // 1. For portable mode, call settings.SetPortableMode()
            if (viewModel.IsPortableMode)
            {
                //settingsManager.SetPortableMode();
            }
            // 2. For custom path, call settings.SetLibraryPath(path)
            else
            {
                settingsManager.SetLibraryPath(viewModel.DataDirectory);
            }
            // Indicate success
            return true;
        }

        return false;
    }



    public void Receive(FileOpenedMessage message)
    {
        //Pages.Add(new OllamaViewModel());
    }
}