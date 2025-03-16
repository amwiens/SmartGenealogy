using Tesseract;

namespace SmartGenealogy.Images.Services;

public class ImageService
{
    public string GetTextFromImage(string imagePath)
    {
        var engine = new TesseractEngine(@"C:\Code\tessdata-4.1.0", "eng");

        var image = Pix.LoadFromFile(imagePath);
        var page = engine.Process(image);

        var text = page.GetText();

        return text;
    }
}