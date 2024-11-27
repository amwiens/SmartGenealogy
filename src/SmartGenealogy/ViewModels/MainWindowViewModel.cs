using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Languages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels;

[View(typeof(MainWindow))]
public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ISettingsManager settingsManager;
    private readonly ServiceManager<ViewModelBase> dialogFactory;
    private readonly INotificationService notificationService;
    public string Greeting => "Welcome to Avalonia!";

    [ObservableProperty]
    private PageViewModelBase? currentPage;

    [ObservableProperty]
    private object? selectedCategory;

    [ObservableProperty]
    private List<PageViewModelBase> pages = new();

    [ObservableProperty]
    private List<PageViewModelBase> footerPages = new();



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
        await base.OnLoadedAsync();

        // Skip if design mode
        if (Design.IsDesignMode)
            return;

        if (!await EnsureDataDirectory())
        {
            // False if user exited dialog, shutdown app
            App.Shutdown();
            return;
        }



        Program.StartupTimer.Stop();
        var startupTime = CodeTimer.FormatTime(Program.StartupTimer.Elapsed);
        Logger.Info($"App started ({startupTime})");
    }

    private void PreloadPages()
    {
        // Preload pages with Preload attribute
        foreach (
            var page in Pages
                .Concat(FooterPages)
                .Where(p => p.GetType().GetCustomAttributes(typeof(PreloadAttribute), true).Any()))
        {
            Dispatcher
                .UIThread.InvokeAsync(
                    async () =>
                    {
                        var stopwatch = Stopwatch.StartNew();

                        page.OnLoaded();
                        await page.OnLoadedAsync();

                        // Get view
                        new ViewLocator().Build(page);

                        Logger.Trace(
                            $"Preloaded page {page.GetType().Name} in {stopwatch.Elapsed.TotalMilliseconds:F1}ms");
                    },
                    DispatcherPriority.Background)
                .ContinueWith(task =>
                {
                    if (task.Exception is { } exception)
                    {
                        Logger.Error(exception, "Error preloading page");
                        Debug.Fail(exception.Message);
                    }
                });
        }
    }

    /// <summary>
    /// Check if the data directory exists, if not, show the select data directory dialog.
    /// </summary>
    /// <returns></returns>
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
            throw new System.Exception("Could not find library after setting path");
        }

        // Tell LaunchPage to load any packages if they selected an existing directory
        if (!foundInitially)
        {

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
                settingsManager.SetPortableMode();
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

    public async Task ShowUpdateDialog()
    {
        var viewModel = dialogFactory.Get<UpdateViewModel>();
        var dialog = new BetterContentDialog
        {
            ContentVerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
            DefaultButton = ContentDialogButton.Close,
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            IsFooterVisible = false,
            Content = new UpdateDialog { DataContext = viewModel }
        };

        await viewModel.Preload();
        await dialog.ShowAsync();
    }
}