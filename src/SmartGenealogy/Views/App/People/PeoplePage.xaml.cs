namespace SmartGenealogy.Views;

public partial class PeoplePage : BasePage
{
    public PeoplePage(PeopleViewModel peopleViewModel)
    {
        InitializeComponent();
        BindingContext = peopleViewModel;
    }
}