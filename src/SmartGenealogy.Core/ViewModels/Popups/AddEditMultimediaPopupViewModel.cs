using Microsoft.Maui.Graphics.Platform;

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
            var fileInfo = new FileInfo(FileName);

            if ((MediaType)MediaType == Data.Enums.MediaType.Image)
            {
                // Read the image as a stream
                using var stream = File.OpenRead(FileName);
                // Load the image using MAUI graphics
                Microsoft.Maui.Graphics.IImage? image = PlatformImage.FromStream(stream);
                byte[]? thumbnailBytes = null;
                if (image != null)
                {
                    // Resize the image to 128x128
                    var resized = image.Resize(128, 128, ResizeMode.Fit);
                    // Save the resized image to a byte array (PNG format)
                    using var ms = new MemoryStream();
                    resized.Save(ms, ImageFormat.Png);
                    thumbnailBytes = ms.ToArray();
                }
                _multimedia.Thumbnail = thumbnailBytes;
            }

            _multimedia.MediaType = (MediaType)MediaType;
            _multimedia.MediaPath = fileInfo.DirectoryName;
            _multimedia.MediaFile = fileInfo.Name;
            _multimedia.Caption = Caption;
            _multimedia.Description = Description;
            _multimedia.Date = Date;
            _multimedia.RefNumber = RefNumber;

            var ocrResult = await ocrService.ProcessImage(FileName);

            var multimediaId = await multimediaService.SaveItemAsync(_multimedia!, ocrResult!.Lines.ToList(), ocrResult.Elements.ToList());

            await popupService.ClosePopupAsync(Shell.Current);
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