using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Media;

[QueryProperty(nameof(IsEdited), "IsEdited")]
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

    [RelayCommand]
    private async Task AddMedia()
    {
        var parameters = new Dictionary<string, object>
        {
        };
        await Shell.Current.GoToAsync("AddMediaPage", parameters);
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