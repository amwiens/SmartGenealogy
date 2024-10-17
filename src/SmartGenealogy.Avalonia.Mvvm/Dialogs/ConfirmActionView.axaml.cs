namespace SmartGenealogy.Avalonia.Mvvm;

public partial class ConfirmActionView : UserControl
{
    public ConfirmActionView()
    {
        this.InitializeComponent();
        this.SetValue(Panel.ZIndexProperty, 999);
        this.Loaded += (_, _) => this.OuterGrid.Opacity = 1.0;
    }
}