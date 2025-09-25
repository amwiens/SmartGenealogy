namespace SmartGenealogy.ViewModels.Tools;

public partial class FactTypesPageViewModel : ObservableObject
{
    private readonly FactTypeRepository _factTypeRepository;

    [ObservableProperty]
    bool _isBusy;

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

        FactTypes = await _factTypeRepository.ListAsync();

        IsBusy = false;
    }

    [RelayCommand]
    private async Task AddFactType()
    {

    }

    [RelayCommand]
    private void OpenFactTypeDetails(FactType factType)
    {
        Shell.Current.GoToAsync($"factTypeDetails?id={factType.Id}");
    }
}