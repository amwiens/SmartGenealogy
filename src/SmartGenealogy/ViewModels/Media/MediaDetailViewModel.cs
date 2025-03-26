using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Media;

[QueryProperty(nameof(MultimediaId), "MultimediaId")]
[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class MediaDetailViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly MultimediaService _multimediaService;

    [ObservableProperty]
    private int multimediaId;

    [ObservableProperty]
    private Multimedia? multimedia;

    [ObservableProperty]
    private bool isEdited;

    [ObservableProperty]
    private string? imagePath;

    public MediaDetailViewModel(MultimediaService multimediaService)
    {
        _multimediaService = multimediaService;
    }

    private async Task LoadMultimediaAsync()
    {
        Multimedia = await _multimediaService.GetMultimediaAsync(MultimediaId);
    }

    partial void OnMultimediaChanged(Multimedia? value)
    {
        ImagePath = Path.Combine(value!.MediaPath!, value!.MediaFile!);
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || Multimedia is null;

        if (shouldRefresh)
        {
            await LoadMultimediaAsync();
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}