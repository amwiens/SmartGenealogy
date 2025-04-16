namespace SmartGenealogy.ViewModels.Settings;

public partial class MainSettingsViewModel : BaseViewModel
{
    [RelayCommand]
    private async Task GoToApplicationSettings()
    {
        await Shell.Current.GoToAsync(nameof(ApplicationSettingsPage));
    }

    [RelayCommand]
    private async Task GoToAISettings()
    {
        await Shell.Current.GoToAsync(nameof(AISettingsPage));
    }

    public async Task OnNavigatedToAsync()
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFromAsync()
    {
        await Task.CompletedTask;
    }
}