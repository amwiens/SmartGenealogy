# SmartGenealogy
This is a project to help with common genealogy tasks. It will help with managing images and places. It is written in .NET MAUI.

## Project setup
This project uses the new solution format. To use this, you might need to enable a preview feature in Visual Studio and other IDEs might not support this format yet. To enable this in Visual Studio, go to Tools -> Options and make the selection based on the image below.

![Visual Studio Options](/assets/images/VSOptions.png "Visual Studio Options")

Tesseract is used for OCR of images. This needs trained language models to be downloaded in order to run which you can download [here](https://github.com/tesseract-ocr/tessdata).

[Geocodio](https://www.geocod.io/) is used for geocoding places within the United States. The API key is set in the settings area of the application.