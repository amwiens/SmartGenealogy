namespace SmartGenealogy.Core.ViewModels.Sources;

/// <summary>
/// Sources page view model.
/// </summary>
/// <param name="sourceRepository">Source repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class SourcesPageViewModel(
    SourceRepository sourceRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Source> _sources = [];

    [ObservableProperty]
    private Source? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        var sourceList = await sourceRepository.ListAsync();
        Sources = new ObservableCollection<Source>(sourceList!);
    }


    [RelayCommand]
    private async Task AddSource()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                //var result = await popupService.ShowPopupAsync<AddEditSourceViewModel>(shell);
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    [RelayCommand]
    private async Task OpenSourceDetails()
    {
        if (SelectedItem is not null)
            await Shell.Current.GoToAsync($"source?id={SelectedItem.Id}");
    }
}