namespace SmartGenealogy.Converters;

public class BooleanToIntegerConverter : IValueConverter, IMarkupExtension
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isGridMode = (bool)value!;

        if (isGridMode)
            return 2;
        else
            return 1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}