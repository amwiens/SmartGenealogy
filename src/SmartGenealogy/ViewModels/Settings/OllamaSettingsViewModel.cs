using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Settings;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(OllamaSettingsPage))]
[RegisterSingleton<OllamaSettingsViewModel>]
[ManagedService]
public partial class OllamaSettingsViewModel : PageViewModelBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ISettingsManager settingsManager;
    private readonly IOllamaService ollamaService;

    public override string Title => "Ollama Settings";
    public override IconSource IconSource => new SymbolIconSource { Symbol = Symbol.Alert, IconVariant = IconVariant.Filled };

    [ObservableProperty]
    private string? _ollamaURL;

    [ObservableProperty]
    private IReadOnlyList<OllamaModelItem> installedModels = [];

    [ObservableProperty]
    private IReadOnlyList<OllamaModel> availableModels = [];

    public OllamaSettingsViewModel(
        ISettingsManager settingsManager,
        IOllamaService ollamaService)
    {
        this.settingsManager = settingsManager;
        this.ollamaService = ollamaService;

        OllamaURL = settingsManager.Settings.OllamaUrl;

        //InstalledModels = GetInstalledModels().OrderBy(item => item.Name).ToImmutableArray();
        //AvailableModels = GetAvailableModels().OrderBy(item => item.Name).ToImmutableArray();
    }

    public override void OnLoaded()
    {
        InstalledModels = GetInstalledModels().OrderBy(item => item.Name).ToImmutableArray();
        //AvailableModels = GetAvailableModels().OrderBy(item => item.Name).ToImmutableArray();
    }

    public override async Task OnLoadedAsync()
    {
        var models = await GetAvailableModelsAsync();
        AvailableModels = models.OrderBy(item => item.Name).ToImmutableArray();
        return;
    }

    private IEnumerable<OllamaModelItem> GetInstalledModels()
    {
        var installedModels = new List<string>
        {
            { "llama3.2" },
            { "llama3.2-vision" }
        };

        foreach (var model in installedModels)
        {
            yield return new OllamaModelItem()
            {
                Name = model
            };
        }
    }

    private IEnumerable<OllamaModelItem> GetAvailableModels()
    {
        //var availableModels1 = await ollamaService.GetOllamaModels().Result;

        var availableModels = new List<string>
        {
            { "llama3.2" },
            { "llama3.2-vision" }
        };

        foreach (var model in availableModels!)
        {
            yield return new OllamaModelItem()
            {
                Name = model
            };
        }
    }
    private async Task<IEnumerable<OllamaModel>> GetAvailableModelsAsync()
    {
        return await ollamaService.GetOllamaModels();

        //var availableModels = new List<string>
        //{
        //    { "llama3.2" },
        //    { "llama3.2-vision" }
        //};

        //foreach (var model in availableModels!)
        //{
        //    yield return new OllamaModelItem()
        //    {
        //        Name = model
        //    };
        //}
    }
}