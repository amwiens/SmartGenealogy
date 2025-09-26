namespace SmartGenealogy.Views.Popups;

public partial class NewDatabasePopupPage : Popup
{
	public NewDatabasePopupPage(NewDatabasePopupPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}