namespace SmartGenealogy.Controls;

public partial class SimpleLoadingIndicator : ContentView
{
    public SimpleLoadingIndicator()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
       nameof(IsBusy),
       typeof(bool),
       typeof(SimpleLoadingIndicator),
       false,
       BindingMode.OneWay,
       null,
       SetIsBusy);

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    private static void SetIsBusy(BindableObject bindable, object oldValue, object newValue)
    {
        SimpleLoadingIndicator? control = bindable as SimpleLoadingIndicator;

        control!.IsVisible = (bool)newValue;
        control.actIndicator.IsRunning = (bool)newValue;
    }
}