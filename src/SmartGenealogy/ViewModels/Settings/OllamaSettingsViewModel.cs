using SmartGenealogy.Ollama;

namespace SmartGenealogy.ViewModels;

public partial class OllamaSettingsViewModel : BaseViewModel
{
    private OllamaService? ollamaService = null;

    #region Ctor

    public OllamaSettingsViewModel()
    {
        ollamaService = new OllamaService(AppSettings.OllamaPath);
    }

    #endregion

    #region Properties

    [ObservableProperty]
    private string? ollamaPath = AppSettings.OllamaPath;

    [ObservableProperty]
    bool? isRunning = false;

    #endregion

    #region Methods

    async partial void OnOllamaPathChanged(string? value)
    {
        AppSettings.OllamaPath = value!;
        ollamaService!.Url = value!;
        IsRunning = ollamaService.IsRunning;
    }

    #endregion
}