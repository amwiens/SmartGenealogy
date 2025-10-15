using SmartGenealogy.Data.Repositories;

namespace SmartGenealogy.Core.ViewModels.Popups;

public partial class SelectMultimediaPopupViewModel(
    MultimediaRepository multimediaRepository,
    IPopupService popupService)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Data.Models.Multimedia> _multimedia = [];

    [ObservableProperty]
    private Data.Models.Multimedia? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        Multimedia = new ObservableCollection<Data.Models.Multimedia>(await multimediaRepository.ListAsync());
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (SelectedItem is null)
            return;

        if (File.Exists(SelectedItem.FullPath))
        {
            await popupService.ClosePopupAsync(Shell.Current, SelectedItem.Id);
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