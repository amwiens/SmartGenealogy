using Microsoft.Maui.Graphics.Platform;

namespace SmartGenealogy.ViewModels.Popups;

public partial class NewMultimediaPopupPageViewModel : ObservableObject
{
    private readonly MultimediaRepository _multimediaRepository;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private string? _filePath;

    [ObservableProperty]
    private string? _caption;

    [ObservableProperty]
    private string? _refNumber;

    [ObservableProperty]
    private string? _date;

    [ObservableProperty]
    private string? _sortDate;

    [ObservableProperty]
    private string? _description;

    public NewMultimediaPopupPageViewModel(MultimediaRepository multimediaRepository, IPopupService popupService)
    {
        _multimediaRepository = multimediaRepository;
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task OpenFile()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Please select a database file",
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                // Process the selected file
                if (File.Exists(result.FullPath))
                {
                    FilePath = result.FullPath;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    [RelayCommand]
    private async Task AddMultimedia()
    {
        if (File.Exists(FilePath))
        {
            var fileInfo = new FileInfo(FilePath);
            // Read the image as a stream
            using var stream = File.OpenRead(FilePath);
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
            var multimedia = new Multimedia
            {
                MediaType = MediaType.Image,
                MediaPath = fileInfo.DirectoryName,
                MediaFile = fileInfo.Name,
                URL = null,
                Thumbnail = thumbnailBytes,
                Caption = Caption,
                RefNumber = RefNumber,
                Date = Date,
                //SortDate = SortDate,
                Description = Description,
            };
            await _multimediaRepository.SaveItemAsync(multimedia);
            // Close the popup after adding
            await _popupService.ClosePopupAsync(Application.Current!.Windows[0].Page!);
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await _popupService.ClosePopupAsync(Application.Current!.Windows[0].Page!);
    }
}