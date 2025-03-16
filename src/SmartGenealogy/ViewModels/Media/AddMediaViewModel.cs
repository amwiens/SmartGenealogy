using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Enums;
using SmartGenealogy.Images.Services;
using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Media;

[QueryProperty(nameof(OwnerId), "OwnerId")]
[QueryProperty(nameof(OwnerType), "OwnerType")]
public partial class AddMediaViewModel : ObservableObject
{
    private readonly MultimediaService _multimediaService;
    private readonly MediaLinkService _mediaLinkService;
    private readonly ImageService _imageService;

    [ObservableProperty]
    private int ownerId;

    [ObservableProperty]
    private OwnerType ownerType;

    [ObservableProperty]
    private string? filePath;

    [ObservableProperty]
    private string? caption;

    [ObservableProperty]
    private string? date;

    [ObservableProperty]
    private string? refNumber;

    [ObservableProperty]
    private string? text;

    public AddMediaViewModel(MultimediaService multimediaService,
        MediaLinkService mediaLinkService,
        ImageService imageService)
    {
        _multimediaService = multimediaService;
        _mediaLinkService = mediaLinkService;
        _imageService = imageService;
    }

    [RelayCommand]
    private void GetText()
    {
        Text = _imageService.GetTextFromImage(FilePath!);
    }

    [RelayCommand]
    private async Task SaveMediaAsync()
    {
        if (File.Exists(FilePath))
        {
            var fileInfo = new FileInfo(FilePath);

            var media = new Multimedia
            {
                MediaPath = fileInfo.DirectoryName,
                MediaFile = fileInfo.Name,
                Caption = Caption,
                Date = Date,
                RefNumber = RefNumber,
                Text = Text,
                DateChanged = DateTime.Now
            };

            await _multimediaService.AddMultimediaAsync(media);

            var mediaLink = new MediaLink
            {
                MediaId = media.Id,
                OwnerType = OwnerType,
                OwnerId = OwnerId,
                DateChanged = DateTime.Now
            };

            await _mediaLinkService.AddMediaLinkAsync(mediaLink);

            var parameters = new Dictionary<string, object>
            {
                { "IsEdited", true }
            };
            await Shell.Current.GoToAsync("///MediaPage", true, parameters);
        }
    }
}