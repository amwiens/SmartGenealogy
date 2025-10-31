namespace SmartGenealogy.Converters;

public class IsNotEmptyStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (String.IsNullOrEmpty(value!.ToString()))
        {
            return false;
        }
        else
            return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}