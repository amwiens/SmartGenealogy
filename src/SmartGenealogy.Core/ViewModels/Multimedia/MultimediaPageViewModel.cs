namespace SmartGenealogy.Core.ViewModels.Multimedia;

/// <summary>
/// Multimedia page view model.
/// </summary>
/// <param name="multimediaRepository">Multimedia repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class MultimediaPageViewModel(
    MultimediaRepository multimediaRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Data.Models.Multimedia> _multimedia = [];

    [ObservableProperty]
    private Data.Models.Multimedia? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        Multimedia = new ObservableCollection<Data.Models.Multimedia>(await multimediaRepository.ListAsync());
    }

    /// <summary>
    /// Add multimedia.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AddMultimedia()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                //var result = await popupService.ShowPopupAsync<AddEditMultimediaPopupViewModel>(shell);
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open multimedia details
    /// </summary>
    [RelayCommand]
    private async Task OpenMultimediaDetails()
    {
        if (SelectedItem is not null)
            await Shell.Current.GoToAsync($"multimediaDetails?id={SelectedItem.Id}");
    }
}