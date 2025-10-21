namespace SmartGenealogy.Controls;

public partial class LanguageItemTemplate : Grid
{
    public LanguageItemTemplate()
    {
        InitializeComponent();
    }

    /* Launguage Name */

    public static BindableProperty NameProperty =
        BindableProperty.Create(
            nameof(Name),
            typeof(string),
            typeof(LanguageItemTemplate),
            string.Empty
        );

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    /* Flag */

    public static BindableProperty FlagProperty =
        BindableProperty.Create(
            nameof(Flag),
            typeof(string),
            typeof(LanguageItemTemplate),
            string.Empty
        );

    public string Flag
    {
        get => (string)GetValue(FlagProperty);
        set => SetValue(FlagProperty, value);
    }
}