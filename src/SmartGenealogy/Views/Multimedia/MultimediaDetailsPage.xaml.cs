namespace SmartGenealogy.Views.Multimedia;

/// <summary>
/// Multimedia details page
/// </summary>
public partial class MultimediaDetailsPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public MultimediaDetailsPage(MultimediaDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}