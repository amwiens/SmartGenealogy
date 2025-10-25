namespace SmartGenealogy.Core.ViewModels.Sources;

/// <summary>
/// Source page view model.
/// </summary>
/// <param name="sourceRepository">Source repository</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class SourcePageViewModel(
    SourceRepository sourceRepository,
    IAlertService alertService,
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
    /// <param name="id">Source identifier</param>
    private async Task LoadData(int id)
    {
        try
        {
            _source = await sourceRepository.GetAsync(id);

            if (_source.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Source with id {id} could not be found."));
                return;
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
    /// Edit source.
    /// </summary>
    [RelayCommand]
    private async Task EditSource()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _source!.Id },
        };

        await popupService.ShowPopupAsync<AddEditSourcePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete source.
    /// </summary>
    [RelayCommand]
    private async Task DeleteSource()
    {
        try
        {
            var isConfirmed = await alertService.ShowAlertAsync("Delete Source", $"Are you sure you want to delete this source?", "Yes", "No");

            if (isConfirmed)
            {
                await sourceRepository.DeleteItemAsync(_source!);
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }
}