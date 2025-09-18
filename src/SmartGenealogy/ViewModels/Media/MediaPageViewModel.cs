namespace SmartGenealogy.ViewModels.Media;

public partial class MediaPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title = "Media Page";

    public MediaPageViewModel()
    {
    }
}