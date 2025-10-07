namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Tools page view model.
/// </summary>
public partial class ToolsPageViewModel : ObservableObject
{
    /// <summary>
    /// Open Fact Types page.
    /// </summary>
    [RelayCommand]
    private async Task OpenFactTypesPage()
    {
        await Shell.Current.GoToAsync("factTypes");
    }
}