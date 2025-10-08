namespace SmartGenealogy.Core.Extensions;

/// <summary>
/// String extensions
/// </summary>
public static class StringExtensions
{
    public static string ReverseString(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string[] parts = text.Split(',', StringSplitOptions.TrimEntries);
        Array.Reverse(parts);
        string reversedString = string.Join(", ", parts);
        return reversedString;
    }
}