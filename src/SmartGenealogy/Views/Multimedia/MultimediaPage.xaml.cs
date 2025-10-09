namespace SmartGenealogy.Views.Multimedia;

/// <summary>
/// Multimedia page
/// </summary>
public partial class MultimediaPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public MultimediaPage(MultimediaPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}