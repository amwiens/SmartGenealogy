namespace SmartGenealogy.Controls;

public partial class ButtonGoogleLogin : ContentView
{
    public ButtonGoogleLogin()
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