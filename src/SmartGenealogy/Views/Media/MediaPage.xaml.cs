namespace SmartGenealogy.Views.Media;

public partial class MediaPage : ContentPage
{
	public MediaPage(MediaPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}