namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add/edit source popup view model.
/// </summary>
public partial class AddEditSourcePopupViewModel(
    SourceRepository sourceRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Source? _source;

    [ObservableProperty]
    public string? _name = string.Empty;

    [ObservableProperty]
    public string? _refNumber = string.Empty;

    [ObservableProperty]
    public string? _actualText = string.Empty;

    [ObservableProperty]
    public string? _comments = string.Empty;

    [ObservableProperty]
    public bool _isPrivate = false;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int sourceId = Convert.ToInt32(query["id"]);
            LoadData(sourceId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Source identifier.</param>
    private async Task LoadData(int id)
    {
        try
        {
            _source = await sourceRepository.GetAsync(id);

            if (_source.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Source with id {id} could not be found."));
            }

            Name = _source!.Name;
            RefNumber = _source.RefNumber;
            ActualText = _source.ActualText;
            Comments = _source.Comments;
            IsPrivate = _source.IsPrivate;
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (_source is null)
            _source = new Source();

        _source.Name = Name ?? string.Empty;
        _source.RefNumber = RefNumber ?? string.Empty;
        _source.ActualText = ActualText ?? string.Empty;
        _source.Comments = Comments ?? string.Empty;
        _source.IsPrivate = IsPrivate;

        var sourceId = await sourceRepository.SaveItemAsync(_source);

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