namespace SmartGenealogy.ViewModels.Settings;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title = "Settings Page";

    public SettingsPageViewModel()
    {
    }
}