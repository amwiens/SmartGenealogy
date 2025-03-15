using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Media;

public partial class MediaViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly MultimediaService _multimediaService;

    [ObservableProperty]
    private ObservableCollection<Multimedia> _multimedia;

    [ObservableProperty]
    private bool isEdited;

    public MediaViewModel(MultimediaService multimediaService)
    {
        _multimediaService = multimediaService;
        Multimedia = new ObservableCollection<Multimedia>();
    }

    private async Task LoadMultimediaAsync()
    {
        var multimedia = await _multimediaService.GetMultimediaAsync();
        Multimedia.Clear();
        foreach (var item in multimedia)
        {
            Multimedia.Add(item);
        }
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || Multimedia.Count == 0;

        if (shouldRefresh)
        {
            await LoadMultimediaAsync();
            IsEdited = false;
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}