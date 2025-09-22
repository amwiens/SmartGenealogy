namespace SmartGenealogy.Views.Media;

public partial class MediaDetailsPage : BasePage
{
	public MediaDetailsPage(MediaDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}