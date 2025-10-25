namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add/Edit source popup
/// </summary>
public partial class AddEditSourcePopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditSourcePopup(AddEditSourcePopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}