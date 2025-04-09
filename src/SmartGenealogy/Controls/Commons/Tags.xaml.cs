namespace SmartGenealogy.Controls;

public partial class Tags : ContentView
{
    public Tags()
    {
        InitializeComponent();
    }

    /* Tag Color */

    public static BindableProperty TagColorProperty =
        BindableProperty.Create(
            nameof(TagColor),
            typeof(Color),
            typeof(Tags),
            defaultValue: ResourceHelper.FindResource<Color>("PrimaryColor")
        );

    public new Color TagColor
    {
        get { return (Color)GetValue(TagColorProperty); }
        set { SetValue(TagColorProperty, value); }
    }

    /* Tag Corner Radius */

    public static BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(Double),
            typeof(Tags),
            defaultValue: 10.0
        );

    public Double CornerRadius
    {
        get { return (Double)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    /* Tag Image */

    public static BindableProperty ImageProperty =
        BindableProperty.Create(
            nameof(Image),
            typeof(ImageSource),
            typeof(Tags),
            defaultValue: null
        );

    public ImageSource Image
    {
        get { return (ImageSource)GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    public static BindableProperty ImageSizeProperty =
        BindableProperty.Create(
            nameof(ImageSize),
            typeof(Double),
            typeof(Tags),
            defaultValue: 20.0
        );

    public Double ImageSize
    {
        get { return (Double)GetValue(ImageSizeProperty); }
        set { SetValue(ImageSizeProperty, value); }
    }

    /* Tag Icon */

    public static BindableProperty IconProperty =
        BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(Tags),
            defaultValue: ""
        );

    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static BindableProperty IconFontSizeProperty =
        BindableProperty.Create(
            nameof(IconFontSize),
            typeof(Double),
            typeof(Tags),
            defaultValue: 10.0
        );

    public Double IconFontSize
    {
        get { return (Double)GetValue(IconFontSizeProperty); }
        set { SetValue(IconFontSizeProperty, value); }
    }

    /* Text */

    public static BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(Tags),
            defaultValue: ""
        );

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static BindableProperty TextColorProperty =
        BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(Tags),
            defaultValue: ResourceHelper.FindResource<Color>("White")
        );

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(Double),
            typeof(Tags),
            defaultValue: 10.0
        );

    public Double FontSize
    {
        get { return (Double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static BindableProperty FontAttributesProperty =
        BindableProperty.Create(
            nameof(FontAttributes),
            typeof(Enum),
            typeof(Tags),
            defaultValue: null
        );

    public Enum FontAttributes
    {
        get { return (Enum)GetValue(FontAttributesProperty); }
        set { SetValue(FontAttributesProperty, value); }
    }
}