using System.Text;

namespace SmartGenealogy.LocationIQ.Models;

public class FreeFormQueryRequest
{
    public string q { get; set; }

    public string format { get; set; }

    public int addressdetails { get; set; }

    public int statecode { get; set; }

    public string viewbox { get; set; }

    public string bounded { get; set; }

    public int limit { get; set; }

    public string acceptlanguage { get; set; }

    public string countrycodes { get; set; }

    public int normalizeaddress { get; set; }

    public int normalizecity { get; set; }

    public int postaladdress { get; set; }

    public int matchquality { get; set; }

    public string source { get; set; }

    public int normalizeimportance { get; set; }

    public int dedupe { get; set; }

    public int namedetails { get; set; }

    public int extratags { get; set; }

    public int polygon_geojson { get; set; }

    public int polygon_kml { get; set; }

    public int polygon_svg { get; set; }

    public int polygon_text { get; set; }

    public string json_callback { get; set; }

    public decimal polygon_threshold { get; set; }

    public override string ToString()
    {
        var querystring = new StringBuilder();

        querystring.Append($"q={q}&");
        if (format != "xml")
            querystring.Append($"format={format}&");
        if (addressdetails != 0)
            querystring.Append($"addressdetails={addressdetails}&");
        if (statecode != 0)
            querystring.Append($"statecode={statecode}&");
        if (!string.IsNullOrWhiteSpace(viewbox))
            querystring.Append($"viewbox={viewbox}&");
        if (!string.IsNullOrWhiteSpace(bounded))
            querystring.Append($"bounded={bounded}&");
        if (limit != 0)
            querystring.Append($"limit={limit}&");
        if (!string.IsNullOrWhiteSpace(acceptlanguage))
            querystring.Append($"accept-language={acceptlanguage}&");
        if (!string.IsNullOrWhiteSpace(countrycodes))
            querystring.Append($"countrycodes={countrycodes}&");
        if (normalizeaddress != 0)
            querystring.Append($"normalizeaddress={normalizeaddress}&");
        if (normalizecity != 0)
            querystring.Append($"normalizecity={normalizecity}&");
        if (postaladdress != 0)
            querystring.Append($"postaladdress={postaladdress}&");
        if (matchquality != 0)
            querystring.Append($"matchquality={matchquality}&");
        if (!string.IsNullOrWhiteSpace(source))
            querystring.Append($"source={source}&");
        if (normalizeimportance != 0)
            querystring.Append($"normalizeimportance={normalizeimportance}&");
        if (dedupe != 0)
            querystring.Append($"dedupe={dedupe}&");
        if (namedetails != 0)
            querystring.Append($"namedetails={namedetails}&");
        if (extratags != 0)
            querystring.Append($"extratags={extratags}&");
        if (polygon_geojson != 0)
            querystring.Append($"polygon_geojson={polygon_geojson}&");
        if (polygon_kml != 0)
            querystring.Append($"polygon_kml={polygon_kml}&");
        if (polygon_svg != 0)
            querystring.Append($"polygon_svg={polygon_svg}&");
        if (polygon_text != 0)
            querystring.Append($"polygon_text={polygon_text}&");
        if (!string.IsNullOrWhiteSpace(json_callback))
            querystring.Append($"json_callback={json_callback}&");
        if (polygon_threshold != 0)
            querystring.Append($"polygon_threshold={polygon_threshold}&");

        return querystring.ToString().TrimEnd('&');
    }
}