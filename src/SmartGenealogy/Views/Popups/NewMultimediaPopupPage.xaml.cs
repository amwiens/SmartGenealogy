namespace SmartGenealogy.Views.Popups;

public partial class NewMultimediaPopupPage : Popup
{
	public NewMultimediaPopupPage(NewMultimediaPopupPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}