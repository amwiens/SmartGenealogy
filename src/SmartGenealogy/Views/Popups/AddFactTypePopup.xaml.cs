namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add fact type popup
/// </summary>
public partial class AddFactTypePopup : Popup
{
	/// <summary>
	/// Constructor
	/// </summary>
	public AddFactTypePopup(AddFactTypePopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}