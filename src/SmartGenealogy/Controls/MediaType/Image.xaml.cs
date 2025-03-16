namespace SmartGenealogy.Controls.MediaType;

public partial class Image : ContentView
{
    public static BindableProperty DescriptionProperty =
        BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(NewspaperArticle),
            string.Empty);

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public Image()
	{
		InitializeComponent();
	}
}