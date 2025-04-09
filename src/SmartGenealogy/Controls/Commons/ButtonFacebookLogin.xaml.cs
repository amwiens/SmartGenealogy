namespace SmartGenealogy.Controls;

public partial class ButtonFacebookLogin : ContentView
{
    public ButtonFacebookLogin()
    {
        InitializeComponent();
    }

    private async void OnButtonTapped(object sender, EventArgs e)
    {
#if !NAVIGATION
        Application.Current.MainPage.DisplayAlert("Button Clicked!", "Please add your function.", "OK");
#endif
    }
}