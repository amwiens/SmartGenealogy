namespace SmartGenealogy.Mobile.Pages;

public partial class DesktopShell
{
	public DesktopShell()
	{
		InitializeComponent();

		BindingContext = new ShellViewModel();
	}
}