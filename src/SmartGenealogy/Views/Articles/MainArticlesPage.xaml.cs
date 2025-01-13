namespace SmartGenealogy.Views;

public partial class MainArticlesPage : BasePage
{
	public MainArticlesPage()
	{
		InitializeComponent();
		BindingContext = new MainArticlesViewModel();
	}
}