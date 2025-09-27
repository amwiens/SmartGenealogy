namespace SmartGenealogy.Data.Models;

public class Multimedia
{
    [PrimaryKey, AutoIncrement]
    public long Id { get; set; }
    public MediaType MediaType { get; set; }
    public string? MediaPath { get; set; }
    public string? MediaFile { get; set; }
    public string? URL { get; set; }
    public byte[]? Thumbnail { get; set; }
    public string? Caption { get; set; }
    public string? RefNumber { get; set; }
    public string? Date { get; set; }
    public long SortDate { get; set; }
    public string? Description { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateChanged { get; set; }

    public ImageSource? ThumbnailBytes
    {
        get
        {
            if (Thumbnail == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(Thumbnail));
        }
    }

    public string? FullPath
    {
        get
        {
            if (string.IsNullOrEmpty(MediaPath) || string.IsNullOrEmpty(MediaFile))
                return null;
            return Path.Combine(MediaPath, MediaFile);
        }
    }
}