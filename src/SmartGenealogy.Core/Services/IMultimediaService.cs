namespace SmartGenealogy.Core.Services;

public interface IMultimediaService
{
    /// <summary>
    /// Retrieves a specific multimedia item by its ID.
    /// </summary>
    /// <param name="id">The ID of the fact type.</param>
    /// <returns>A <see cref="Multimedia"/> object if found; otherwise, null.</returns>
    Task<Multimedia?> GetAsync(int id);

    /// <summary>
    /// Saves a multimedia item to the database.
    /// </summary>
    /// <param name="multimedia">Multimedia item.</param>
    /// <param name="lines">Lines from the image.</param>
    /// <param name="ocrElement">Words from the image.</param>
    /// <returns>The Id of the saved multimedia.</returns>
    Task<int> SaveItemAsync(Multimedia multimedia, List<string> lines, List<OcrResult.OcrElement> ocrElement);


    Task<int> SaveItemAsync(Multimedia multimedia, string fileName, MediaType mediaType, string? caption, string? description, string? date, string? refNumber);

    /// <summary>
    /// Delete Multimedia item.
    /// </summary>
    /// <param name="item">Multimedia item.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteMultimediaItemAsync(Multimedia item);
}