using System.ComponentModel.DataAnnotations;

namespace SmartGenealogy.Converters;

internal static class EnumAttributeConverters
{
    public static EnumAttributeConverter<DisplayAttribute> Display => new();

    public static EnumAttributeConverter<DisplayAttribute> DisplayName => new(attribute => attribute.Name);

    public static EnumAttributeConverter<DisplayAttribute> DisplayDescription =>
        new(attribute => attribute.Description);
}