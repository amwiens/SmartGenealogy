namespace SmartGenealogy.Views;

public partial class PeoplePage : ContentPage
{
    public PeoplePage()
    {
        InitializeComponent();
        BindingContext = new PeopleViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}