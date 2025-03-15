namespace SmartGenealogy.Geocodio;

/// <summary>
/// A class which holds and validates the Geocodio datafield keys used when querying additional data
/// items such as Census or School district information.
/// </summary>
public class GeocodioDataFieldSettings
{
    public static readonly IEnumerable<string> ValidGeocodioFields = new HashSet<string>()
    {
        "cd",
        "cd113",
        "cd114",
        "cd115",
        "cd116",
        "cd117",
        "cd118",
        "cd119",
        "stateleg",
        "stateleg-next",
        "school",
        "census",
        "census2000",
        "census2010",
        "census2011",
        "census2012",
        "census2013",
        "census2014",
        "census2015",
        "census2016",
        "census2017",
        "census2018",
        "census2019",
        "census2020",
        "census2021",
        "census2022",
        "census2023",
        "census2024",
        "acs-demographics",
        "acs-economics",
        "acs-families",
        "acs-housing",
        "acs-social",
        "zip4",
        "timezone",
        // Canadian items
        "riding",
        "riding-next",
        "provriding",
        "provriding-next",
        "statcan"
    };

    private Dictionary<string, bool> _fieldSettings { get; set; }

    private GeocodioDataFieldSettings(Dictionary<string, bool> fieldSettings)
    {
        _fieldSettings = fieldSettings;
    }

    /// <summary>
    /// Create a fields object to use when sending requests to Geocodio. This object controls which
    /// data fields (census, Congress, etc.) are queried when sending requests.
    /// </summary>
    /// <param name="defaultAllFieldsStatusToInclude"></param>
    /// <returns></returns>
    public static GeocodioDataFieldSettings CreateDataFieldSettings(bool defaultAllFieldsStatusToInclude = false)
    {
        var fieldSettings = new Dictionary<string, bool>();

        if (defaultAllFieldsStatusToInclude)
        {
            foreach (var key in ValidGeocodioFields)
            {
                fieldSettings.Add(key, defaultAllFieldsStatusToInclude);
            }
        }
        else
        {
            foreach (var key in ValidGeocodioFields)
            {
                fieldSettings.Add(key, defaultAllFieldsStatusToInclude);
            }
        }

        return new GeocodioDataFieldSettings(fieldSettings);
    }

    public void SetFieldQueryStatus(string fieldKey, bool includeWhenQuerying)
    {
        this[fieldKey] = includeWhenQuerying;
    }

    public bool GetFieldQueryStatus(string fieldKey)
    {
        return this[fieldKey];
    }

    public bool this[string fieldKey]
    {
        get
        {
            if (ValidGeocodioFields.Contains(fieldKey))
            {
                return _fieldSettings[fieldKey];
            }
            else
            {
                throw new InvalidOperationException($"Field key {fieldKey} is not currently a known valid Geocodio data field key.");
            }
        }
        set
        {
            if (ValidGeocodioFields.Contains(fieldKey))
            {
                _fieldSettings[fieldKey] = value;
            }
            else
            {
                throw new InvalidOperationException($"Field key {fieldKey} is not currently a known valid Geocodio data field key.");
            }
        }
    }
}