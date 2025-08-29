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
        await Application.Current!.Windows[0]!.Page!.DisplayAlert("Button Clicked!", "Please add your function.", "OK");
#endif
    }
}