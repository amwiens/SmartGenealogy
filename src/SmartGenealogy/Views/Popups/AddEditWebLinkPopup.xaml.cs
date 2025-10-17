namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add edit web link popup.
/// </summary>
public partial class AddEditWebLinkPopup : Popup
{
    /// <summary>
    /// Constructory
    /// </summary>
    public AddEditWebLinkPopup(AddEditWebLinkPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}