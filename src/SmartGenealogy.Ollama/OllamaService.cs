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
        get
        {
            CheckIsRunning().ConfigureAwait(false);
            return isRunning;
        }
    }

    public OllamaService(string url)
    {
        Url = url;
        CheckIsRunning();
    }

    private async Task CheckIsRunning()
    {
        bool isRunning;

        try
        {
            isRunning = await ollama!.IsRunningAsync();
        }
        catch
        {
            isRunning = false;
        }

        this.isRunning = isRunning;
    }

    public async Task<IEnumerable<Model>> GetLocalModels()
    {
        await CheckIsRunning();
        if (isRunning)
            return await ollama!.ListLocalModelsAsync();

        return null;
    }
}