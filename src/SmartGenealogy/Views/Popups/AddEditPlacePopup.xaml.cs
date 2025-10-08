namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add/Edit place popup
/// </summary>
public partial class AddEditPlacePopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditPlacePopup(AddEditPlacePopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}