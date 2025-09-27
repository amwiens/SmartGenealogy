using Microsoft.Maui.Graphics.Platform;

namespace SmartGenealogy.ViewModels.Media;

public partial class MediaPageViewModel : ObservableObject
{
    private readonly MultimediaRepository _multimediaRepository;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    bool _isBusy;

    [ObservableProperty]
    private List<Multimedia> _multimedia = [];

    public MediaPageViewModel(MultimediaRepository multimediaRepository, IPopupService popupService)
    {
        _multimediaRepository = multimediaRepository;
        _popupService = popupService;
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
        var popupResult = await _popupService.ShowPopupAsync<NewMultimediaPopupPage>(Application.Current!.Windows[0].Page!);
    }
}