namespace SmartGenealogy.Controls.TintMask;

public class TintColorMask : Image, ITintColorMask
{
    public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(ITintColorMask), defaultValue: Microsoft.Maui.Graphics.Colors.Black);

    public virtual Color TintColor
    {
        get => (Color)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }
}