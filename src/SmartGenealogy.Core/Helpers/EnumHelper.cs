using System.Reflection;

namespace SmartGenealogy.Core.Helpers;

public static class EnumHelper
{
    /// <summary>
    /// Get the description for an enum value.
    /// </summary>
    /// <param name="value">Enum value.</param>
    /// <returns>Description</returns>
    public static string GetEnumDescription(Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttr = member?.GetCustomAttribute<DescriptionAttribute>();
        if (value.ToString() == "0")
            return string.Empty;
        return descriptionAttr?.Description ?? value.ToString();
    }

    /// <summary>
    /// Gets a list of descriptions for the enum type T.
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <returns>List of descriptions</returns>
    public static List<string> GetEnumDescriptions<T>() where T : Enum
    {
        var type = typeof(T);
        var descriptions = Enum.GetValues(type)
            .Cast<T>()
            .Select(e =>
            {
                var member = type.GetMember(e.ToString()).FirstOrDefault();
                var descriptionAttr = member?.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttr?.Description ?? e.ToString();
            })
            .ToList();

        return descriptions;
    }

    /// <summary>
    /// Gets a list of descriptions for the enum type T, including a blank item at the top.
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <returns>List of descriptions</returns>
    public static List<string> GetEnumDescriptionsWithBlank<T>() where T : Enum
    {
        var type = typeof(T);
        var descriptions = Enum.GetValues(type)
            .Cast<T>()
            .Select(e =>
            {
                var member = type.GetMember(e.ToString()).FirstOrDefault();
                var descriptionAttr = member?.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttr?.Description ?? e.ToString();
            })
            .ToList();

        descriptions.Insert(0, string.Empty); // Add blank item at the top
        return descriptions;
    }
}