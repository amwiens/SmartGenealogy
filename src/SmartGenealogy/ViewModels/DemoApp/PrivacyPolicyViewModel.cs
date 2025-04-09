namespace SmartGenealogy.ViewModels;

public partial class PrivacyPolicyViewModel : ObservableObject
{
    [ObservableProperty]
    private string _url;

    public PrivacyPolicyViewModel()
    {
        Url = "http://tlssoftwarevn.com/mauikit-privacy.html";
    }
}