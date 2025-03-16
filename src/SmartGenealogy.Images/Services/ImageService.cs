using Tesseract;

namespace SmartGenealogy.Images.Services;

public class ImageService
{
    public string GetTextFromImage(string imagePath, string languageDataPath)
    {
        var text = string.Empty;

        if (!string.IsNullOrEmpty(languageDataPath))
        {
            var engine = new TesseractEngine(languageDataPath, "eng");

            var image = Pix.LoadFromFile(imagePath);
            var page = engine.Process(image);

            text = page.GetText();

            engine.Dispose();
        }

        return text;
    }
}