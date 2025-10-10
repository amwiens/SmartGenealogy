namespace SmartGenealogy.Core.ViewModels.Multimedia;

/// <summary>
/// Multimedia details page view model.
/// </summary>
/// <param name="multimediaService">Multimedia service</param>
/// <param name="popupService">Popup service</param>
/// <param name="ocrService">OCR Service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class MultimediaDetailsPageViewModel(
    IMultimediaService multimediaService,
    IPopupService popupService,
    OCRService ocrService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Data.Models.Multimedia? _multimedia;

    [ObservableProperty]
    private MediaType _mediaType = 0;

    [ObservableProperty]
    private string? _fileName = string.Empty;

    [ObservableProperty]
    private string? _caption = string.Empty;

    [ObservableProperty]
    private string? _description = string.Empty;

    [ObservableProperty]
    private string? _date = string.Empty;

    [ObservableProperty]
    private string? _refNumber = string.Empty;

    [ObservableProperty]
    private string? _allText = string.Empty;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int multimediaId = Convert.ToInt32(query["id"]);
            LoadData(multimediaId).FireAndForgetSafeAsync();
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
            _multimedia = await multimediaService.GetAsync(id);

            if (_multimedia.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
                return;
            }

            MediaType = _multimedia.MediaType;
            FileName = _multimedia.FullPath;
            Caption = _multimedia.Caption;
            Description = _multimedia.Description;
            Date = _multimedia.Date;
            RefNumber = _multimedia.RefNumber;
            AllText = _multimedia.AllText;
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Edit multimedia.
    /// </summary>
    [RelayCommand]
    private async Task EditMultimedia()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _multimedia!.Id }
        };

        await popupService.ShowPopupAsync<AddEditMultimediaPopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete multimedia.
    /// </summary>
    [RelayCommand]
    private async Task DeleteMultimedia()
    {
        try
        {
            await multimediaService.DeleteMultimediaItemAsync(_multimedia!);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }


    [RelayCommand]
    private async Task ProcessImage()
    {
        var result = await ocrService.ProcessImage(FileName);

        if (result != null && result.Success)
        {
            AllText = result.AllText;
            _multimedia!.AllText = AllText;
            await multimediaService.SaveItemAsync(_multimedia, result.Lines.ToList(), result.Elements.ToList());
        }
    }
}