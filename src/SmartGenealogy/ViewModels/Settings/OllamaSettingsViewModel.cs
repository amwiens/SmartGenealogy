using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using NLog;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
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

    public override string Title => "Ollama Settings";
    public override IconSource IconSource => new SymbolIconSource { Symbol = Symbol.Alert, IconVariant = IconVariant.Filled };

    [ObservableProperty]
    private string? _ollamaURL;

    [ObservableProperty]
    private IReadOnlyList<OllamaModelItem> installedModels = [];

    [ObservableProperty]
    private IReadOnlyList<OllamaModelItem> availableModels = [];

    public OllamaSettingsViewModel(
        ISettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;

        OllamaURL = settingsManager.Settings.OllamaUrl;

        InstalledModels = GetInstalledModels().OrderBy(item => item.Name).ToImmutableArray();
        AvailableModels = GetAvailableModels().OrderBy(item => item.Name).ToImmutableArray();
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
        var availableModels = new List<string>
        {
            { "deepseek-r1" },
            { "llama3.3" }
        };

        foreach (var model in availableModels)
        {
            yield return new OllamaModelItem()
            {
                Name = model
            };
        }
    }
}