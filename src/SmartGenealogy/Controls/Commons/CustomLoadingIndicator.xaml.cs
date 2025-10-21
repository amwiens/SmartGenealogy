namespace SmartGenealogy.Controls;

public partial class CustomLoadingIndicator : ContentView
{
    public CustomLoadingIndicator()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
       nameof(IsBusy),
       typeof(bool),
       typeof(CustomLoadingIndicator),
       false,
       BindingMode.OneWay,
       null,
       SetIsBusy);

    public bool IsBusy
    {
        get => (bool)this.GetValue(IsBusyProperty);
        set => this.SetValue(IsBusyProperty, value);
    }

    private static void SetIsBusy(BindableObject bindable, object oldValue, object newValue)
    {
        CustomLoadingIndicator? control = bindable as CustomLoadingIndicator;

        control!.IsVisible = (bool)newValue;
        control.actIndicator.IsVisible = (bool)newValue;
    }
}