namespace SmartGenealogy.Controls;

public partial class FlyoutFooter : ContentView
{
    public FlyoutFooter()
    {
        InitializeComponent();

        BindingContext = this;
    }

    public string AppVersion { get; set; } = AppInfo.Current.VersionString;
}