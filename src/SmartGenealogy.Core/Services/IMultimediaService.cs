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

    /// <summary>
    /// Saves a multimedia item to the database.
    /// </summary>
    /// <param name="multimedia">Multimedia item</param>
    /// <param name="fileName">File name</param>
    /// <param name="mediaType"><see cref="MediaType"/></param>
    /// <param name="caption">Caption</param>
    /// <param name="description">Description</param>
    /// <param name="date">Date</param>
    /// <param name="refNumber">Reference number</param>
    /// <returns></returns>
    Task<int> SaveItemAsync(Multimedia multimedia, string fileName, MediaType mediaType, string? caption, string? description, string? date, string? refNumber);

    /// <summary>
    /// Delete Multimedia item.
    /// </summary>
    /// <param name="item">Multimedia item.</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    Task<bool> DeleteMultimediaItemAsync(Multimedia item);
}