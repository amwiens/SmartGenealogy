using Microsoft.Maui.Graphics.Platform;

namespace SmartGenealogy.ViewModels.Media;

public partial class MediaPageViewModel : ObservableObject
{
    private readonly MultimediaRepository _multimediaRepository;

    [ObservableProperty]
    bool _isBusy;

    [ObservableProperty]
    private List<Multimedia> _multimedia = [];

    public MediaPageViewModel(MultimediaRepository multimediaRepository)
    {
        _multimediaRepository = multimediaRepository;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        IsBusy = true;

        Multimedia = await _multimediaRepository.ListAsync();

        IsBusy = false;
    }

    [RelayCommand]
    private void OpenMediaDetails()
    {
        // Navigate to the MediaDetailsPage
        Shell.Current.GoToAsync("mediaDetails");
    }

    [RelayCommand]
    private async Task AddMultimedia()
    {
        var filePath = @"";

        if (File.Exists(filePath))
        {
            var fileInfo = new FileInfo(filePath);

            // Read the image as a stream
            using var stream = File.OpenRead(filePath);

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
                Thumbnail = thumbnailBytes,
                Caption = "",
                //RefNumber = "Ref001",
                Date = "",
                SortDate = 0,
                Description = "Description",
            };

            await _multimediaRepository.SaveItemAsync(multimedia);
        }

        Multimedia = await _multimediaRepository.ListAsync();
    }
}