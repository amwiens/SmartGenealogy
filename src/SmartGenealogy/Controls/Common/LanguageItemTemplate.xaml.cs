namespace SmartGenealogy.Controls;

public partial class LanguageItemTemplate : Grid
{
	public LanguageItemTemplate()
	{
		InitializeComponent();
	}

	public static BindableProperty NameProperty =
		BindableProperty.Create(
			nameof(Name),
			typeof(string),
			typeof(LanguageItemTemplate),
			string.Empty);

	public string Name
	{
		get { return (string)GetValue(NameProperty); }
		set { SetValue(NameProperty, value); }
	}

	public static BindableProperty FlagProperty =
		BindableProperty.Create(
			nameof(Flag),
			typeof(string),
			typeof(LanguageItemTemplate),
			string.Empty);

	public string Flag
	{
		get { return (string)GetValue(FlagProperty); }
		set { SetValue(FlagProperty, value); }
	}
}