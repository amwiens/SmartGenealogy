namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Select location popup
/// </summary>
public partial class SelectLocationPopup : Popup<int>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Select location popup view model</param>
    public SelectLocationPopup(SelectLocationPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}