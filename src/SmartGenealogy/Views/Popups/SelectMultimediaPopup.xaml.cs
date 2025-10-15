namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Select multimedia popup
/// </summary>
public partial class SelectMultimediaPopup : Popup<int>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Select multimedia popup view model</param>
    public SelectMultimediaPopup(SelectMultimediaPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}