namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add edit multimedia popup view model.
/// </summary>
/// <param name="multimediaService">Multimedia service</param>
/// <param name="ocrService">OCR service</param>
/// <param name="popupService">PopupService</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class AddEditMultimediaPopupViewModel(
    IMultimediaService multimediaService,
    OCRService ocrService,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Data.Models.Multimedia? _multimedia;

    [ObservableProperty]
    private int _mediaType = 0;

    [ObservableProperty]
    private List<string> _mediaTypes = Enum.GetNames<MediaType>().ToList();

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

            MediaType = (int)_multimedia.MediaType;
            FileName = _multimedia.FullPath;
            Caption = _multimedia.Caption;
            Description = _multimedia.Description;
            Date = _multimedia.Date;
            RefNumber = _multimedia.RefNumber;
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Select media.
    /// </summary>
    [RelayCommand]
    private async Task SelectMedia()
    {
        FilePickerFileType? fileTypes;
        try
        {
            switch (MediaType)
            {
                case (int)Data.Enums.MediaType.Image:
                    fileTypes = FilePickerFileType.Images;
                    break;

                case (int)Data.Enums.MediaType.Video:
                    fileTypes = FilePickerFileType.Videos;
                    break;

                default:
                    fileTypes = null;
                    break;
            }

            var options = new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = fileTypes
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                if (File.Exists(result.FullPath))
                {
                    FileName = result.FullPath;
                }
            }
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
        if (_multimedia is null)
            _multimedia = new Data.Models.Multimedia();

        if (File.Exists(FileName))
        {
            var multimediaId = await multimediaService.SaveItemAsync(_multimedia!, FileName, (MediaType)MediaType, Caption, Description, Date, RefNumber);

            await popupService.ClosePopupAsync(Shell.Current, multimediaId);
        }
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