namespace SmartGenealogy.Views.Media;

public partial class MediaPage : BasePage
{
	public MediaPage(MediaPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}