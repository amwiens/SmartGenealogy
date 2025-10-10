namespace SmartGenealogy.Core.Services;

public class MultimediaService(
    MultimediaRepository multimediaRepository,
    MultimediaLineRepository multimediaLineRepository,
    MultimediaWordRepository multimediaWordRepository)
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
    public async Task<int> SaveItemAsync(Multimedia multimedia, List<string> lines, List<OcrResult.OcrElement> ocrElement)
    {
        var multimediaId = 0;
        try
        {
            multimediaId = await multimediaRepository.SaveItemAsync(multimedia);
            var lineNumber = 0;
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
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteMultimediaItemAsync(Multimedia item)
    {
        var linesDeleted = await multimediaLineRepository.DeleteItemAsync(item.Id);
        var wordsDeleted = await multimediaWordRepository.DeleteItemAsync(item.Id);
        var mediaDeleted = await multimediaRepository.DeleteItemAsync(item);

        return linesDeleted + wordsDeleted + mediaDeleted;
    }
}