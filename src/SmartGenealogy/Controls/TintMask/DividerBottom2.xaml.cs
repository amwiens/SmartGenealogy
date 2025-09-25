namespace SmartGenealogy.Controls.TintMask;

public partial class DividerBottom2 : Grid
{
    public static BindableProperty MaskColorProperty =
            BindableProperty.Create(
                nameof(MaskColor),
                typeof(Color),
                typeof(DividerBottom2),
                defaultValue: Color.FromArgb("#00FFFFFF"),
                propertyChanged: OnMaskColorChanged
            );

    public Color MaskColor
    {
        get => (Color)GetValue(MaskColorProperty);
        set => SetValue(MaskColorProperty, value);
    }

    public DividerBottom2()
    {
        InitializeComponent();
    }

    private static void OnMaskColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((DividerBottom2)bindable).Update();
    }

    private void Update()
    {
        if (root.Children != null)
        {
            root.Children.Clear();
        }

        if (MaskColor != Color.FromArgb("#00FFFFFF"))
        {
            var image = new TintColorMask
            {
                Style = Resources["MaskImageStyle"] as Style
            };
            root.Children!.Add(image);
        }
    }
}