namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add/Edit project popup
/// </summary>
public partial class AddEditProjectPopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditProjectPopup(AddEditProjectPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}