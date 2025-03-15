using System.ComponentModel;

namespace SmartGenealogy.Enums;

public enum PlaceDetailType
{
    [Description("Cemetery")]
    Cemetery,

    [Description("Church")]
    Church,

    [Description("Newspaper")]
    Newspaper,

    [Description("School")]
    School,

    [Description("Other")]
    Other,

    [Description("Home")]
    Home,

    [Description("Business")]
    Business,

    [Description("Hospital")]
    Hospital,
}