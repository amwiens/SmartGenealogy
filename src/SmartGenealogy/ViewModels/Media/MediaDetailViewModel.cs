using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Media;

[QueryProperty(nameof(MultimediaId), "MultimediaId")]
[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class MediaDetailViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly MultimediaService _multimediaService;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private int multimediaId;

    [ObservableProperty]
    private Multimedia? multimedia;

    [ObservableProperty]
    private bool isEdited;

    //[ObservableProperty]
    //private string? imagePath;

    public MediaDetailViewModel(MultimediaService multimediaService, IPopupService popupService)
    {
        _multimediaService = multimediaService;
        _popupService = popupService;
    }

    private async Task LoadMultimediaAsync()
    {
        Multimedia = await _multimediaService.GetMultimediaAsync(MultimediaId);
    }

    //partial void OnMultimediaChanged(Multimedia? value)
    //{
    //    ImagePath = Path.Combine(value!.MediaPath!, value!.MediaFile!);
    //}

    [RelayCommand]
    private async Task EditMediaDetail()
    {
        //_popupService.
        bool answer = await Application.Current!.Windows[0].Page!.DisplayAlert(
    "Delete Place",
    $"You you sure you want to delete {Multimedia!.Caption}?",
    "Yes", "No");
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