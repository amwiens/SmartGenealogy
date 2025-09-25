namespace SmartGenealogy.Converters;

public class BooleanToFlowDirectionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isRightToLeft = value != null ? (bool)value : false;

        if (isRightToLeft)
        {
            return FlowDirection.RightToLeft;
        }
        else
            return FlowDirection.LeftToRight;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}