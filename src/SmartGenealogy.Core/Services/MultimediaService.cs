namespace SmartGenealogy.Core.Services;

/// <summary>
/// Multimedia Service
/// </summary>
/// <param name="multimediaRepository">Multimedia repository</param>
/// <param name="multimediaLineRepository">Multimedia line repository</param>
/// <param name="multimediaWordRepository">Multimedia word repository</param>
/// <param name="mediaLinkRepository">Media link repository</param>
/// <param name="alertService">Alert service</param>
/// <param name="ocrService">OCR service</param>
public class MultimediaService(
    MultimediaRepository multimediaRepository,
    MultimediaLineRepository multimediaLineRepository,
    MultimediaWordRepository multimediaWordRepository,
    MediaLinkRepository mediaLinkRepository,
    IAlertService alertService,
    OCRService ocrService)
    : IMultimediaService
{
    /// <summary>
    /// Retrieves a specific multimedia item by its ID.
    /// </summary>
    /// <param name="id">The ID of the fact type.</param>
    /// <returns>A <see cref="Multimedia"/> object if found; otherwise, null.</returns>
    public async Task<Multimedia?> GetAsync(int id)
    {
        return await multimediaRepository.GetAsync(id);
    }

    /// <summary>
    /// Saves a multimedia item to the database.
    /// </summary>
    /// <param name="multimedia">Multimedia item.</param>
    /// <param name="lines">Lines from the image.</param>
    /// <param name="ocrElement">Words from the image.</param>
    /// <returns>The Id of the saved multimedia.</returns>
    public async Task<int> SaveItemAsync(Multimedia multimedia, List<string>? lines = null, List<OcrResult.OcrElement>? ocrElement = null)
    {
        var multimediaId = 0;
        try
        {
            if (multimedia.Id == 0)
            {
                var existingMultimedia = await multimediaRepository.GetAsync(multimedia.MediaPath!, multimedia.MediaFile!);
                if (existingMultimedia != null && existingMultimedia.Id != 0)
                {
                    await Toast.Make("Multimedia item is already in the database.").Show();
                    return existingMultimedia.Id;
                }
            }

            multimediaId = await multimediaRepository.SaveItemAsync(multimedia);
            var lineNumber = 0;
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    var multimediaLine = new MultimediaLine
                    {
                        MultimediaId = multimediaId,
                        LineNumber = lineNumber,
                        Text = line
                    };
                    await multimediaLineRepository.SaveItemAsync(multimediaLine);
                    lineNumber++;
                }
            }
            if (ocrElement != null)
            {
                foreach (var element in ocrElement)
                {
                    var multimediaWord = new MultimediaWord
                    {
                        MultimediaId = multimediaId,
                        Confidence = element.Confidence,
                        Height = element.Height,
                        Text = element.Text,
                        Width = element.Width,
                        X = element.X,
                        Y = element.Y
                    };
                    await multimediaWordRepository.SaveItemAsync(multimediaWord);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return multimediaId;
    }

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
    public async Task<int> SaveItemAsync(Multimedia multimedia, string fileName, MediaType mediaType, string? caption, string? description, string? date, string? refNumber)
    {
        var multimediaId = 0;
        try
        {
            if (File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);

                // Check if multimedia already exists in the database.
                if (multimedia.Id == 0)
                {
                    var existingMultimedia = await multimediaRepository.GetAsync(fileInfo.DirectoryName!, fileInfo.Name);
                    if (existingMultimedia != null && existingMultimedia.Id != 0)
                    {
                        await Toast.Make("Multimedia item is already in the database.").Show();
                        return existingMultimedia.Id;
                    }
                }

                // Get image thumbnail
                if (mediaType == MediaType.Image)
                {
                    // Read the image as a stream
                    using var stream = File.OpenRead(fileName);
                    // Load the image using MAUIgraphics
                    Microsoft.Maui.Graphics.IImage? image = PlatformImage.FromStream(stream);
                    byte[]? thumbnailBytes = null;
                    if (image != null)
                    {
                        // Resize the image to 128x128
                        var resized = image.Resize(128, 128, ResizeMode.Fit);
                        // Save the resized image to a byte array (PNG format)
                        using var ms = new MemoryStream();
                        resized.Save(ms, ImageFormat.Png);
                        thumbnailBytes = ms.ToArray();
                    }
                    multimedia.Thumbnail = thumbnailBytes;
                }

                multimedia.MediaType = mediaType;
                multimedia.MediaPath = fileInfo.DirectoryName;
                multimedia.MediaFile = fileInfo.Name;
                multimedia.Caption = caption;
                multimedia.Description = description;
                multimedia.Date = date;
                multimedia.RefNumber = refNumber;

                // Save multimedia item
                multimediaId = await multimediaRepository.SaveItemAsync(multimedia);

                if (mediaType == MediaType.Image)
                {
                    var ocrResult = await ocrService.ProcessImage(fileName);

                    // Delete existing lines and words from tables
                    await multimediaLineRepository.DeleteItemAsync(multimediaId);
                    await multimediaWordRepository.DeleteItemAsync(multimediaId);

                    // Insert lines from OCR
                    var lineNumber = 0;
                    if (ocrResult!.Lines != null)
                    {
                        foreach (var line in ocrResult.Lines)
                        {
                            var multimediaLine = new MultimediaLine
                            {
                                MultimediaId = multimediaId,
                                LineNumber = lineNumber,
                                Text = line
                            };
                            await multimediaLineRepository.SaveItemAsync(multimediaLine);
                            lineNumber++;
                        }
                    }
                    // Insert words from OCR
                    if (ocrResult.Elements != null)
                    {
                        foreach (var element in ocrResult.Elements)
                        {
                            var multimediaWord = new MultimediaWord
                            {
                                MultimediaId = multimediaId,
                                Confidence = element.Confidence,
                                Height = element.Height,
                                Text = element.Text,
                                Width = element.Width,
                                X = element.X,
                                Y = element.Y
                            };
                            await multimediaWordRepository.SaveItemAsync(multimediaWord);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return multimediaId;
    }

    /// <summary>
    /// Delete Multimedia item.
    /// </summary>
    /// <param name="item">Multimedia item.</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    public async Task<bool> DeleteMultimediaItemAsync(Multimedia item)
    {
        if (item == null)
            return false;

        var mediaLinks = await mediaLinkRepository.ListAsync(item.Id);

        bool isConfirmed = true;

        var deletionMessage = new StringBuilder();
        deletionMessage.AppendLine("This multimedia item has the following links:");
        deletionMessage.AppendLine();

        // Check for media links
        if (mediaLinks!.Any())
        {
            deletionMessage.AppendLine("* Media links");
            isConfirmed = false;
        }

        deletionMessage.AppendLine();
        deletionMessage.AppendLine("Do you still want to delete this multimedia item?");

        // Check if confirmation box needs to surface
        if (!isConfirmed)
            isConfirmed = await alertService.ShowAlertAsync("Delete multimedia", deletionMessage.ToString(), "Yes", "No");

        // Delete items
        if (isConfirmed)
        {
            var linksDeleted = await mediaLinkRepository.DeleteItemAsync(item.Id);
            var linesDeleted = await multimediaLineRepository.DeleteItemAsync(item.Id);
            var wordsDeleted = await multimediaWordRepository.DeleteItemAsync(item.Id);
            var mediaDeleted = await multimediaRepository.DeleteItemAsync(item);

            return true;
        }

        return false;
    }
}