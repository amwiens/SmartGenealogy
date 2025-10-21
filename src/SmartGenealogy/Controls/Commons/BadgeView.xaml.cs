namespace SmartGenealogy.Controls;

public partial class BadgeView : ContentView
{
    public BadgeView()
    {
        InitializeComponent();
    }

    public static BindableProperty AutoHideProperty =
            BindableProperty.Create(
                nameof(AutoHide),
                typeof(bool),
                typeof(BadgeView),
                defaultValue: true,
                propertyChanged: (bindable, oldValue, newValue) => ((BadgeView)bindable).UpdateVisibility()
            );

    public bool AutoHide
    {
        get => (bool)GetValue(AutoHideProperty);
        set => SetValue(AutoHideProperty, value);
    }

    private void UpdateVisibility()
    {
        IsVisible = !AutoHide ||
            (!string.IsNullOrWhiteSpace(LabelText.Text) && LabelText.Text.Trim() != "0");
    }

    public new static BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(BadgeView),
            defaultValue: ResourceHelper.FindResource<Color>("PrimaryColor"),
            propertyChanged: (bindable, oldValue, newValue) => ((BadgeView)bindable).badgeShape.BackgroundColor = (Color)newValue
        );

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public static BindableProperty TextColorProperty =
        BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(BadgeView),
            defaultValue: ResourceHelper.FindResource<Color>("White"),
            propertyChanged: (bindable, oldValue, newValue) => ((BadgeView)bindable).LabelText.TextColor = (Color)newValue
        );

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(BadgeView),
            defaultValue: "",
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var badge = (BadgeView)bindable;
                badge.LabelText.Text = (string)newValue;
                badge.UpdateVisibility();
            }
        );

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}