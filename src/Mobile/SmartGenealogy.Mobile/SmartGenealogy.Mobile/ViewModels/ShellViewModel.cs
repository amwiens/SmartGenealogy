using SmartGenealogy.Mobile.Resources.Strings;

namespace SmartGenealogy.Mobile.ViewModels;

public class ShellViewModel : ViewModelBase
{
    public AppSection Home { get; set; }

    public ShellViewModel()
    {
        Home = new AppSection() { Title = AppResource.Home, Icon = "home.png", IconDark = "home_dark.png", TargetType = typeof(HomePage) };
    }
}