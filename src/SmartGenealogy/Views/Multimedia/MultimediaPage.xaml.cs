namespace SmartGenealogy.Views.Multimedia;

/// <summary>
/// Multimedia page
/// </summary>
public partial class MultimediaPage : BasePage
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