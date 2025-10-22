namespace SmartGenealogy.Controls;

public partial class AvatarWithStatus : ContentView
{
    public AvatarWithStatus()
    {
        InitializeComponent();
    }

    public static BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(ImageSource),
                typeof(AvatarWithStatus),
                defaultValue: null
            );

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static BindableProperty StatusProperty =
        BindableProperty.Create(
            nameof(Status),
            typeof(string),
            typeof(AvatarWithStatus),
            defaultValue: default(AvatarStatus).ToString()
        );

    public string Status
    {
        get => (string)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static BindableProperty AvatarHeightProperty =
            BindableProperty.Create(
                nameof(AvatarHeight),
                typeof(double),
                typeof(AvatarWithStatus),
                0.0,
                BindingMode.OneWay,
                validateValue: (_, value) => value != null);

    public double AvatarHeight
    {
        get => (double)GetValue(AvatarHeightProperty);
        set => SetValue(AvatarHeightProperty, value);
    }

    public static BindableProperty AvatarWidthProperty =
            BindableProperty.Create(
                nameof(AvatarWidth),
                typeof(double),
                typeof(AvatarWithStatus),
                0.0,
                BindingMode.OneWay,
                validateValue: (_, value) => value != null);

    public double AvatarWidth
    {
        get => (double)GetValue(AvatarWidthProperty);
        set => SetValue(AvatarWidthProperty, value);
    }

    public static BindableProperty CornerRadiusProperty =
       BindableProperty.Create(
           nameof(CornerRadius),
           typeof(Double),
           typeof(AvatarWithStatus),
           defaultValue: 10.0
       );

    public Double CornerRadius
    {
        get => (Double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    // Convenience property
    public AvatarStatus StatusEnum
    {
        get
        {
            return Enum.TryParse(Status, out AvatarStatus result) ?
                result :
                default(AvatarStatus);
        }

        set { Status = value.ToString(); }
    }
}