namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add/edit role popup
/// </summary>
public partial class AddEditRolePopup : Popup
{
	/// <summary>
	/// Constructor
	/// </summary>
	public AddEditRolePopup(AddEditRolePopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}