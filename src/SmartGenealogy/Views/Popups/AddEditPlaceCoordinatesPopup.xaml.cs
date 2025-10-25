namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add/Edit place popup
/// </summary>
public partial class AddEditPlaceCoordinatesPopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditPlaceCoordinatesPopup(AddEditPlaceCoordinatesPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}