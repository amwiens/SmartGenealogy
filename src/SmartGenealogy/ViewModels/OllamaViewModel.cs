using System.Collections.ObjectModel;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using Injectio.Attributes;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(OllamaPage))]
[RegisterSingleton<OllamaViewModel>]
public partial class OllamaViewModel : PageViewModelBase
{
    public override string Title => "Ollama page";

    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Laptop, IconVariant = IconVariant.Filled };

    public string LanguageLimitationWarning { get; } = string.Format("Currently the only supported model is {0}", "llama3.2");
    public string ResourceLimitWarning { get; } = string.Format("This model may not run optimally because you have {0} GB of VRAM, and this model requires {1} GB at least", 69, 420);

    private IOllamaService _ollamaService;

    [ObservableProperty]
    private ObservableCollection<Message> messages = new();

    [ObservableProperty]
    private string newMessageText = string.Empty;

    public OllamaViewModel(IOllamaService ollamaService)
    {
        _ollamaService = ollamaService;
    }

    [RelayCommand]
    private async Task SendMessage()
    {
        if (NewMessageText.Length == 0) return;
        NewMessageText = NewMessageText.Trim();
        Messages.Add(new Message(NewMessageText));
        var tmp = NewMessageText;
        NewMessageText = string.Empty;
        await AddGeneratedMessage(tmp);
    }

    private async Task AddGeneratedMessage(string prompt)
    {
        var generatedMessage = await _ollamaService.GenerateMessage(prompt);
        if (generatedMessage != null)
        {
            Messages.Add(generatedMessage);
        }
    }
}