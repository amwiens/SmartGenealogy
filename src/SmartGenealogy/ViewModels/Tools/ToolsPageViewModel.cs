namespace SmartGenealogy.ViewModels.Tools;

public partial class ToolsPageViewModel : ObservableObject
{

    [RelayCommand]
    private void OpenFactTypes()
    {
        // Navigate to the FactTypesPage
        Shell.Current.GoToAsync("factTypes");
    }
}