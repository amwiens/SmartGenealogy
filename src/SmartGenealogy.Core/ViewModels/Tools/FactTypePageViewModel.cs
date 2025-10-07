namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Fact type page view model.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="modalErrorHandler">Modal error handler</param>
public partial class FactTypePageViewModel(FactTypeRepository factTypeRepository, IPopupService popupService, ModalErrorHandler modalErrorHandler) : ObservableObject, IQueryAttributable
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
    /// <returns></returns>
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

    [RelayCommand]
    private async Task EditFactType()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _factType.Id }
        };

        await popupService.ShowPopupAsync<AddEditFactTypePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    [RelayCommand]
    private async Task DeleteFactType()
    {
        try
        {
            await factTypeRepository.DeleteItemAsync(_factType!);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            modalErrorHandler.HandleError(ex);
        }
    }
}