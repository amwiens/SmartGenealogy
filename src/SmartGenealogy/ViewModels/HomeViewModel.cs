using System;
using System.Threading.Tasks;

using FluentAvalonia.UI.Controls;
using FluentIcons.Common;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Languages;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.ViewModels.Dialogs;
using SmartGenealogy.Views;
using SmartGenealogy.Views.Dialogs;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(HomePage))]
[Singleton]
public partial class HomeViewModel : PageViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private INotificationService notificationService;
    private ISettingsManager settingsManager;
    private IProjectSettingsManager projectSettingsManager;

    private readonly ServiceManager<ViewModelBase> dialogFactory;

    public override string Title => Resources.Label_Home;
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Home, IconVariant = IconVariant.Filled };

    public HomeViewModel(
        INotificationService notificationService,
        ISettingsManager settingsManager,
        IProjectSettingsManager projectSettingsManager,

        ServiceManager<ViewModelBase> dialogFactory)
    {
        this.notificationService = notificationService;
        this.settingsManager = settingsManager;
        this.projectSettingsManager = projectSettingsManager;

        this.dialogFactory = dialogFactory;
    }

    public async Task CreateNewProject()
    {
        var viewModel = dialogFactory.Get<CreateProjectViewModel>();
        var dialog = new BetterContentDialog
        {
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            IsFooterVisible = false,
            Content = new CreateProjectDialog { DataContext = viewModel }
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            projectSettingsManager.Settings.Name = viewModel.ProjectName;
            projectSettingsManager.SetProjectPath(viewModel.DataDirectory);
            //settingsManager.Settings.
        }
    }
}