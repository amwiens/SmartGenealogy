using System.Collections.ObjectModel;

using OllamaSharp.Models;

using SmartGenealogy.Ollama;

namespace SmartGenealogy.ViewModels;

public partial class OllamaSettingsViewModel : BaseViewModel
{
    private OllamaService? ollamaService = null;

    #region Ctor

    public OllamaSettingsViewModel()
    {
        ollamaService = new OllamaService(AppSettings.OllamaPath);
        ollamaService!.Url = AppSettings.OllamaPath;
        LoadData().ConfigureAwait(false);
    }

    #endregion

    #region Properties

    [ObservableProperty]
    private string? ollamaPath = AppSettings.OllamaPath;

    [ObservableProperty]
    bool? isRunning;

    public ObservableCollection<Model> LocalModels { get; private set; } = new();

    #endregion

    #region Commands


    #endregion

    #region Methods

    public async Task LoadData()
    {
        if (isRefreshing)
            return;

        try
        {
            isRefreshing = true;
            IsRunning = ollamaService!.IsRunning;
            var models = await ollamaService!.GetLocalModels();

            foreach (var model in models)
                LocalModels.Add(model);
            IsRunning = ollamaService!.IsRunning;
        }
        finally
        {
            isRefreshing = false;
        }
    }

    async partial void OnOllamaPathChanged(string? value)
    {
        AppSettings.OllamaPath = value!;
        ollamaService!.Url = value!;
        IsRunning = ollamaService.IsRunning;
    }

    #endregion
}