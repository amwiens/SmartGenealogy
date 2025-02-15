using System.Globalization;

using Avalonia;
using Avalonia.Media;

namespace SmartGenealogy.Controls;

public class DynamicSvg : Avalonia.Svg.Svg
{
    public static readonly StyledProperty<IBrush?> FillColorProperty =
        AvaloniaProperty.Register<DynamicSvg, IBrush?>("FillColor");

    public IBrush? FillColor
    {
        get => GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public DynamicSvg(IServiceProvider provider) : base(provider) { }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == FillColorProperty)
        {
            var color = Colors.Transparent;
            var colorParse = Color.TryParse(FillColor?.ToString(), out color);
            var rgb = color.ToUInt32();
            var result = $"{rgb.ToString("x8", CultureInfo.InvariantCulture)}";
            result = result.Substring(2);
            if (colorParse)
            {
                SetCurrentValue(CssProperty, $"* {{ fill: #{result}; stroke: #{result}");
            }
        }
    }
}