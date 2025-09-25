using SmartGenealogy.Data.Models;

namespace SmartGenealogy.ViewModels.Tools;

public partial class FactTypesPageViewModel : ObservableObject
{
    private readonly FactTypeRepository _factTypeRepository;

    [ObservableProperty]
    private List<FactType> _factTypes = [];

    public FactTypesPageViewModel(FactTypeRepository factTypeRepository)
    {
        _factTypeRepository = factTypeRepository;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        FactTypes = await _factTypeRepository.ListAsync();
    }
}