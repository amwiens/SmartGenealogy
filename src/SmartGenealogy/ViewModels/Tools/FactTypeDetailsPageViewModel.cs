namespace SmartGenealogy.ViewModels.Tools;

public partial class FactTypeDetailsPageViewModel : ObservableObject, IQueryAttributable
{
    private FactType? _factType;
    private readonly FactTypeRepository _factTypeRepository;
    private readonly ModalErrorHandler _errorHandler;

    [ObservableProperty]
    bool _isBusy;

    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private string? _abbreviation = string.Empty;

    [ObservableProperty]
    private string? _gedcomTag = string.Empty;

    [ObservableProperty]
    private bool _useValue;

    [ObservableProperty]
    private bool _useDate;

    [ObservableProperty]
    private bool _usePlace;

    [ObservableProperty]
    private string? _sentence = string.Empty;

    [ObservableProperty]
    private bool _isBuiltIn;

    public FactTypeDetailsPageViewModel(FactTypeRepository factTypeRepository, ModalErrorHandler errorHandler)
    {
        _factTypeRepository = factTypeRepository;
        _errorHandler = errorHandler;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int factTypeId = Convert.ToInt32(query["id"]);
            LoadData(factTypeId).FireAndForgetSafeAsync();
        }
    }

    private async Task RefreshData()
    {
        if (_factType.IsNullOrNew())
        {
            return;
        }

        await LoadData(_factType.Id);
    }

    private async Task LoadData(long id)
    {
        try
        {
            IsBusy = true;

            _factType = await _factTypeRepository.GetAsync(id);

            if (_factType.IsNullOrNew())
            {
                _errorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
                return;
            }

            Name = _factType.Name;
            Abbreviation = _factType.Abbreviation;
            GedcomTag = _factType.GedcomTag;
            UseValue = _factType.UseValue;
            UseDate = _factType.UseDate;
            UsePlace = _factType.UsePlace;
            Sentence = _factType.Sentence;
            IsBuiltIn = _factType.IsBuiltIn;
        }
        catch (Exception ex)
        {
            _errorHandler.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditFactType()
    {

    }

    [RelayCommand]
    private async Task DeleteFactType()
    {
        await _factTypeRepository.DeleteItemAsync(_factType!.Id);
        await Shell.Current.GoToAsync("..");
    }
}