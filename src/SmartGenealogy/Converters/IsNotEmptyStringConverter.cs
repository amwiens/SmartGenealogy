﻿using System.Globalization;

namespace SmartGenealogy.Converters;

public class IsNotEmptyStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value.ToString()))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}