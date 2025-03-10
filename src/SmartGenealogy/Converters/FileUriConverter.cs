﻿using System;
using System.Globalization;

using Avalonia.Data.Converters;

using SmartGenealogy.Core.Extensions;

namespace SmartGenealogy.Converters;

public class FileUriConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(Uri))
        {
            return null;
        }

        return value switch
        {
            string str when str.StartsWith("avares://") => new Uri(str),
            string str => new Uri("file://" + str),
            IFormattable formattable => new Uri("file://" + formattable.ToString(null, culture)),
            _ => null
        };
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(string) && value is Uri uri)
        {
            return uri.ToString().StripStart("file://");
        }

        return null;
    }
}