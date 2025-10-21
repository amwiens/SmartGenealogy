using IImage = Microsoft.Maui.IImage;

namespace SmartGenealogy.Controls.TintMask;

public interface ITintColorMask : IImage
{
    /// <summary>
    /// Tint color of an image.
    /// </summary>
    Color TintColor { get; set; }
}