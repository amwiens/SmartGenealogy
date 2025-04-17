namespace SmartGenealogy.Views.Popups.Settings;

public partial class LanguageSelectionPopupPage : BasePopupPage
{
    public LanguageSelectionPopupPage()
    {
        InitializeComponent();
        BindingContext = new LanguageSelectionPopupViewModel();
    }
}