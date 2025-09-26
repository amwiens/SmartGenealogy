namespace SmartGenealogy.Controls;

public partial class FlyoutHeader : ContentView
{
    public FlyoutHeader()
    {
        InitializeComponent();

        BindingContext = this;
    }

    public static BindableProperty DatabaseNameProperty =
            BindableProperty.Create(
                nameof(DatabaseName),
                typeof(string),
                typeof(FlyoutHeader),
                string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (FlyoutHeader)bindable;
                    ctrl.databaseName.Text = (string)newValue;
                }
            );

    public string DatabaseName
    {
        get => (string)GetValue(DatabaseNameProperty);
        set => SetValue(DatabaseNameProperty, value);
    }
}