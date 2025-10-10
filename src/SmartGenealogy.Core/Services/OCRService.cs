namespace SmartGenealogy.Core.Services;

/// <summary>
/// Ocr Service
/// </summary>
/// <param name="ocrService">Ocr Service</param>
public class OCRService(IOcrService ocrService)
{
    /// <summary>
    /// Gets the text from an image.
    /// </summary>
    /// <param name="imagePath">Image path</param>
    /// <returns>An <see cref="OcrResult"/> object if successful, otherwise <see langword="null"/>.</returns>
    public async Task<OcrResult?> ProcessImage(string? imagePath)
    {
        if (imagePath == null)
            return null;

        if (File.Exists(imagePath))
        {
            byte[] imageBytes = imagePath.FileToByteArray();
            var result = await ocrService.RecognizeTextAsync(imageBytes);
            if (result != null && result.Success)
                return result;
        }

        return null;
    }
}