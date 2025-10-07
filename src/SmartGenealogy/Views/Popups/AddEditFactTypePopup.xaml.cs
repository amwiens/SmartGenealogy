namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add fact type popup
/// </summary>
public partial class AddEditFactTypePopup : Popup
{
	/// <summary>
	/// Constructor
	/// </summary>
	public AddEditFactTypePopup(AddEditFactTypePopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}