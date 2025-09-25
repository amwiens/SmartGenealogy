namespace SmartGenealogy.Converters;

public class TaskPriorityToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string resourceName;

        var stringValue = value != null ? value.ToString() : "";

        switch (stringValue)
        {
            case "Low":
                resourceName = "Green";
                break;

            case "High":
                resourceName = "Red";
                break;

            case "Mid":
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