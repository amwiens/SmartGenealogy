namespace SmartGenealogy.Views.Settings;

public partial class ApplicationSettingsPage : BasePage
{
    private readonly ApplicationSettingsViewModel _viewModel;

    public ApplicationSettingsPage(ApplicationSettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.OnNavigatedToAsync();
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await _viewModel.OnNavigatedFromAsync();
    }
}