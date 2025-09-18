namespace SmartGenealogy.ViewModels.Main;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title = "Main Page";

    public MainPageViewModel()
    {
    }
}