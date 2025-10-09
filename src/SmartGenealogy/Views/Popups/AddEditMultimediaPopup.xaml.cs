namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add multimedia popup
/// </summary>
public partial class AddEditMultimediaPopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditMultimediaPopup(AddEditMultimediaPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}