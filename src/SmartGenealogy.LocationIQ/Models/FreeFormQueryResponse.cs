namespace SmartGenealogy.LocationIQ.Models;

public class FreeFormQueryResponse
{
    public string place_id { get; set; }
    public string licence { get; set; }
    public string osm_type { get; set; }
    public string osm_id { get; set; }
    public string[] boundingbox { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public string display_name { get; set; }
    public string _class { get; set; }
    public string type { get; set; }
    public float importance { get; set; }
    public string icon { get; set; }
    public Address address { get; set; }
    public GeoJson geojson { get; set; }
    public string geokml { get; set; }
    public string svg { get; set; }
    public string geotext { get; set; }
    public ExtraTags extratags { get; set; }
    public NameDetails namedetails { get; set; }
}

public class Address
{
    public string attraction { get; set; }
    public string house_number { get; set; }
    public string road { get; set; }
    public string neighbourhood { get; set; }
    public string suburb { get; set; }
    public string county { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string state_code { get; set; }
    public string postcode { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }
}

public class GeoJson
{
    public string type { get; set; }
    public float[][][] coordinates { get; set; }
}

public class ExtraTags
{
    public string ele { get; set; }
    public string capital { get; set; }
    public string website { get; set; }
    public string wikidata { get; set; }
    public string wikipedia { get; set; }
    public string population { get; set; }
    public string border_type { get; set; }
    public string designation { get; set; }
    public string linked_place { get; set; }
    public string populationdate { get; set; }
    public string sourcepopulation { get; set; }
}

public class NameDetails
{
    public string name { get; set; }
    public string nameen { get; set; }
    public string _place_nameen { get; set; }
}
