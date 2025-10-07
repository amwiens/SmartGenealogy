namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add edit fact type popup view model.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="popupService">PopupService</param>
/// <param name="modalErrorHandler">Modal error handler</param>
public partial class AddEditFactTypePopupViewModel(
    FactTypeRepository factTypeRepository,
    IPopupService popupService,
    ModalErrorHandler modalErrorHandler)
    : ObservableObject, IQueryAttributable
{
    private FactType? _factType;

    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private string? _abbreviation = string.Empty;

    [ObservableProperty]
    private string? _gedcomTag = string.Empty;

    [ObservableProperty]
    private bool _useValue = false;

    [ObservableProperty]
    private bool _useDate = false;

    [ObservableProperty]
    private bool _usePlace = false;

    [ObservableProperty]
    private string? _sentence = string.Empty;

    [ObservableProperty]
    private bool _isBuiltIn = false;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int factTypeId = Convert.ToInt32(query["id"]);
            LoadData(factTypeId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Record identifier</param>
    private async Task LoadData(int id)
    {
        try
        {
            _factType = await factTypeRepository.GetAsync(id);

            if (_factType.IsNullOrNew())
            {
                modalErrorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
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
            modalErrorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (_factType is null)
            _factType = new FactType();

        _factType.Name = Name;
        _factType.Abbreviation = Abbreviation;
        _factType.GedcomTag = GedcomTag;
        _factType.UseValue = UseValue;
        _factType.UseDate = UseDate;
        _factType.UsePlace = UsePlace;
        _factType.Sentence = Sentence;

        var factTypeId = await factTypeRepository.SaveItemAsync(_factType);

        await popupService.ClosePopupAsync(Shell.Current);
    }

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await popupService.ClosePopupAsync(Shell.Current);
    }
}