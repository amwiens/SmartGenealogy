namespace SmartGenealogy.Views;

public partial class PeoplePage : ContentPage
{
    public PeoplePage(PeopleViewModel peopleViewModel)
    {
        InitializeComponent();
        BindingContext = peopleViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}