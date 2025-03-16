namespace SmartGenealogy.Controls.MediaType;

public partial class NewspaperArticle : ContentView
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

	public NewspaperArticle()
	{
		InitializeComponent();
	}

    private void text_TextChanged(object sender, TextChangedEventArgs e)
    {
		Description = $"newspaper:{txtNewspaper.Text};page:{txtPage.Text}";
    }
}