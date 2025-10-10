namespace SmartGenealogy.Core.Extensions;

/// <summary>
/// String extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Reverses a string by comma delimiter
    /// </summary>
    /// <param name="text">Text to reverse</param>
    /// <returns>Reversed string.</returns>
    public static string ReverseString(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string[] parts = text.Split(',', StringSplitOptions.TrimEntries);
        Array.Reverse(parts);
        string reversedString = string.Join(", ", parts);
        return reversedString;
    }

    /// <summary>
    /// Converts an image to byte[]
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <returns>byte array</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public static byte[] FileToByteArray(this string filePath)
    {
        // Validate the file path
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist.", filePath);
        }

        // Read the file into a byte array
        return File.ReadAllBytes(filePath);
    }
}