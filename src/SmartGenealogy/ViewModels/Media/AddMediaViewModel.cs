using System.Threading.Tasks;

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
    private readonly OllamaService _ollamaService;

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

    [ObservableProperty]
    private string? mediaType;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? summary;

    public List<string> MediaTypes { get; } = Enum.GetNames<MediaType>().Order().ToList();

    public AddMediaViewModel(MultimediaService multimediaService,
        MediaLinkService mediaLinkService,
        ImageService imageService,
        OllamaService ollamaService)
    {
        _multimediaService = multimediaService;
        _mediaLinkService = mediaLinkService;
        _imageService = imageService;
        _ollamaService = ollamaService;
    }

    [RelayCommand]
    private async Task PickFileAsync()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a file",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                FilePath = result.FullPath;
            }
        }
        catch { }
    }

    [RelayCommand]
    private void GetText()
    {
        var languageFilePath = SettingsManager.LoadSettings().TesseractLanguageFileLocation;

        Text = _imageService.GetTextFromImage(FilePath!, languageFilePath!);
    }

    [RelayCommand]
    private async Task GetSummary()
    {
        var prompt = $"You are an experienced genealogist. Using the following obituary, who is it about and who are the children: {Text}";
        var generatedMessage = new GeneratedMessage("", 0.0);

        var messageHistory = new List<Message>
        {
            new Message(prompt)
        };
        await foreach (var chunk in _ollamaService.GenerateMessage(messageHistory))
        {
            if (chunk.Message != null) Summary += chunk.Message.Content;
        }
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
                Description = Description,
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