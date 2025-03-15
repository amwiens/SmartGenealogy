using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.SchoolDistrict;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class SchoolDistrict
{
    private SchoolDistrict() { }

    public SchoolDistrict(string name, string lea_code, string gradeLow, string gradeHigh)
    {
        Name = name;
        LEA_Code = lea_code;
        GradeLow = gradeLow;
        GradeHigh = gradeHigh;
    }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("lea_code")]
    public string? LEA_Code { get; set; }
    [JsonPropertyName("grade_low")]
    public string? GradeLow { get; set; }
    [JsonPropertyName("grade_high")]
    public string? GradeHigh { get; set; }
}