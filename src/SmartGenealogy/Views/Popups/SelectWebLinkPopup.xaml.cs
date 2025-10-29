namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Select web link popup
/// </summary>
public partial class SelectWebLinkPopup : Popup<int>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Select web link popup view model</param>
    public SelectWebLinkPopup(SelectWebLinkPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}