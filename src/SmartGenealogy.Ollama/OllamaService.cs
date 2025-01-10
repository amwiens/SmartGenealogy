using OllamaSharp;
using OllamaSharp.Models;

namespace SmartGenealogy.Ollama;

public class OllamaService
{
    private OllamaApiClient? ollama = null;
    private string url;
    private bool isRunning = false;

    public string? Url
    {
        get => url;
        set
        {
            url = value!;
            ollama = new OllamaApiClient(value!);
            CheckIsRunning().ConfigureAwait(false);
        }
    }

    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public OllamaService(string url)
    {
        Url = url;
    }

    private async Task CheckIsRunning()
    {
        try
        {
            isRunning = await ollama!.IsRunningAsync();
        }
        catch
        {
            isRunning = false;
        }
    }

    public async Task<IEnumerable<Model>> GetLocalModels()
    {
        if (isRunning)
            return await ollama!.ListLocalModelsAsync();

        return null;
    }
}