namespace SmartGenealogy.Views.Popups.App;

public partial class CreateFilePopupPage : BasePopupPage
{
	public CreateFilePopupPage()
	{
		InitializeComponent();
        BindingContext = new CreateFilePopupViewModel();
    }
}