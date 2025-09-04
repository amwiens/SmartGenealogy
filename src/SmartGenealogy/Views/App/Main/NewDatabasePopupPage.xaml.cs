namespace SmartGenealogy.Views;

public partial class NewDatabasePopupPage : BasePopupPage
{
	public NewDatabasePopupPage()
	{
		InitializeComponent();
        BindingContext = new NewDatabasePopupViewModel();
    }
}