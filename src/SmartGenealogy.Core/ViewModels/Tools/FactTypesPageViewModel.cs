namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Fact types page view model
/// </summary>
/// <param name="factTypeRepository">Fact Type repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="modalErrorHandler">Modal error handler</param>
public partial class FactTypesPageViewModel(FactTypeRepository factTypeRepository, IPopupService popupService, ModalErrorHandler modalErrorHandler) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FactType> _factTypes = [];

    [ObservableProperty]
    private FactType? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        FactTypes = new ObservableCollection<FactType>(await factTypeRepository.ListAsync());
    }

    /// <summary>
    /// Add fact type.
    /// </summary>
    [RelayCommand]
    private async Task AddFactType()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<AddFactTypePopupViewModel>(shell);
            }
        }
        catch (Exception ex)
        {
            modalErrorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open Fact type details
    /// </summary>
    [RelayCommand]
    private async Task OpenFactTypeDetails()
    {
        if (SelectedItem is not null)
            await Shell.Current.GoToAsync($"factTypeDetails?id={SelectedItem.Id}");
    }
}