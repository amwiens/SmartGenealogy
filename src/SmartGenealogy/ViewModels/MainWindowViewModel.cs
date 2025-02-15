using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using SmartGenealogy.Languages;
using SmartGenealogy.Messages;

namespace SmartGenealogy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IRecipient<FileOpenedMessage>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

    public MainWindowViewModel()
    {
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
    }

    public void Receive(FileOpenedMessage message)
    {
        //Pages.Add(new OllamaViewModel());
    }
}