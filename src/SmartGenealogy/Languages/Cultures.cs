using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;

using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Languages;

public static class Cultures
{
    public static CultureInfo Default { get; } = new("en-US");

    public static CultureInfo? Current => Resources.Culture;

    public static NumberFormatInfo CurrentNumberFormat => Thread.CurrentThread.CurrentCulture.NumberFormat;

    public static readonly Dictionary<string, CultureInfo> SupportedCulturesByCode =
        new()
        {
            ["en-US"] = Default
        };

    public static IReadOnlyList<CultureInfo> SupportedCultures =>
        SupportedCulturesByCode.Values.ToImmutableList();

    public static CultureInfo GetSupportedCultureOrDefault(string? cultureCode)
    {
        if (cultureCode is null || !SupportedCulturesByCode.TryGetValue(cultureCode, out var culture))
        {
            return Default;
        }

        return culture;
    }

    public static void SetSupportedCultureOrDefault(string? cultureCode, NumberFormatInfo numberFormat)
    {
        if (!TrySetSupportedCulture(cultureCode, numberFormat))
        {
            TrySetSupportedCulture(Default, numberFormat);
        }
    }

    public static void SetSupportedCultureOrDefault(string? cultureCode, NumberFormatMode numberFormatMode)
    {
        if (!TrySetSupportedCulture(cultureCode, numberFormatMode))
        {
            TrySetSupportedCulture(Default, numberFormatMode);
        }
    }

    public static bool TrySetSupportedCulture(string? cultureCode)
    {
        if (cultureCode is null || !SupportedCulturesByCode.TryGetValue(cultureCode, out var culture))
        {
            return false;
        }

        if (Current?.Name != culture.Name)
        {
            Resources.Culture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            EventManager.Instance.OnCultureChanged(culture);
        }

        return true;
    }

    public static bool TrySetSupportedCulture(string? cultureCode, NumberFormatInfo numberFormat)
    {
        if (cultureCode is null || !SupportedCulturesByCode.TryGetValue(cultureCode, out var culture))
        {
            return false;
        }

        if (Current?.Name != culture.Name || CurrentNumberFormat != numberFormat)
        {
            Resources.Culture = culture;

            var cultureInfo = GetCultureInfoWithNumberFormat(culture, numberFormat);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            EventManager.Instance.OnCultureChanged(culture);
        }

        return true;
    }

    public static bool TrySetSupportedCulture(string? cultureCode, NumberFormatMode numberFormatMode)
    {
        if (cultureCode is null || !SupportedCulturesByCode.TryGetValue(cultureCode, out var culture))
        {
            return false;
        }

        var numberFormat = numberFormatMode switch
        {
            NumberFormatMode.CurrentCulture => culture.NumberFormat,
            NumberFormatMode.InvariantCulture => CultureInfo.InvariantCulture.NumberFormat,
            _ => culture.NumberFormat,
        };

        if (Current?.Name != culture.Name || CurrentNumberFormat != numberFormat)
        {
            Resources.Culture = culture;

            var cultureInfo = GetCultureInfoWithNumberFormat(culture, numberFormat);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            EventManager.Instance.OnCultureChanged(culture);
        }

        return true;
    }

    public static bool TrySetSupportedCulture(CultureInfo? cultureInfo)
    {
        return cultureInfo is not null && TrySetSupportedCulture(cultureInfo.Name);
    }

    public static bool TrySetSupportedCulture(CultureInfo? cultureInfo, NumberFormatInfo numberFormat)
    {
        return cultureInfo is not null && TrySetSupportedCulture(cultureInfo.Name, numberFormat);
    }

    public static bool TrySetSupportedCulture(CultureInfo? cultureInfo, NumberFormatMode numberFormatMode)
    {
        return cultureInfo is not null && TrySetSupportedCulture(cultureInfo.Name, numberFormatMode);
    }

    private static CultureInfo GetCultureInfoWithNumberFormat(
        CultureInfo culture,
        NumberFormatInfo numberFormat)
    {
        ArgumentNullException.ThrowIfNull(culture);

        var cultureInfo = (CultureInfo)culture.Clone();
        cultureInfo.NumberFormat = numberFormat;
        return cultureInfo;
    }
}