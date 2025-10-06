namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Tools page view model.
/// </summary>
public partial class ToolsPageViewModel : ObservableObject
{


    [RelayCommand]
    private async Task OpenFactTypesPage()
    {
        await Shell.Current.GoToAsync("factTypes");
    }
}