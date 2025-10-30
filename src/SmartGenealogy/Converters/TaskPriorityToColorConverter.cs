using SmartGenealogy.Data.Enums;

namespace SmartGenealogy.Converters;

public class TaskPriorityToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string resourceName;

        var stringValue = value != null ? value.ToString() : "";

        switch (value)
        {
            case Priority.Low:
                resourceName = "Green";
                break;

            case Priority.High:
                resourceName = "Red";
                break;

            case Priority.Mid:
                resourceName = "Orange";
                break;

            default:
                resourceName = "DisabledColor";
                break;
        }

        return ResourceHelper.FindResource<Color>(resourceName);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}