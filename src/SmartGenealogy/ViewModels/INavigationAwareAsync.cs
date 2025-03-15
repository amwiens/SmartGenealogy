namespace SmartGenealogy.ViewModels;

public interface INavigationAwareAsync
{
    Task OnNavigatedToAsync();

    Task OnNavigatedFromAsync();
}