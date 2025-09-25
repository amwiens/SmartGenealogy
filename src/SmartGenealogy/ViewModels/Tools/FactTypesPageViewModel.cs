namespace SmartGenealogy.ViewModels.Tools;

public partial class FactTypesPageViewModel : ObservableObject
{
    private readonly FactTypeRepository _factTypeRepository;

    [ObservableProperty]
    bool _isBusy;

    [ObservableProperty]
    bool  _isNotBusy;

    [ObservableProperty]
    private List<FactType> _factTypes = [];

    public FactTypesPageViewModel(FactTypeRepository factTypeRepository)
    {
        _factTypeRepository = factTypeRepository;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        IsBusy = true;
        IsNotBusy = false;

        FactTypes = await _factTypeRepository.ListAsync();

        IsBusy = false;
        IsNotBusy = true;
    }

    [RelayCommand]
    private void OpenFactTypeDetails(FactType factType)
    {
        Shell.Current.GoToAsync($"factTypeDetails?id={factType.Id}");
    }
}