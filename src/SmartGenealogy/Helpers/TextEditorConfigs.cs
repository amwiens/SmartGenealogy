using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AvaloniaEdit;

using SmartGenealogy.Models;

using TextMateSharp.Grammars;
using TextMateSharp.Registry;

namespace SmartGenealogy.Helpers;

public static class TextEditorConfigs
{
    public static void Configure(TextEditor editor, TextEditorPreset preset)
    {
        switch (preset)
        {
            case TextEditorPreset.Prompt:
                ConfigForPrompt(editor);
                break;

            case TextEditorPreset.Console:
                ConfigForConsole(editor);
                break;

            case TextEditorPreset.None:
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(preset), preset, null);
        }
    }

    private static void ConfigForPrompt(TextEditor editor)
    {
        const ThemeName themeName = ThemeName.DimmedMonokai;
        var registryOptions = new RegistryOptions(themeName);

        var registry = new Registry(registryOptions);

        using var stream = Assets.ImagePromptLanguageJson.Open();
        var promptGrammar = registry.LoadGrammarFromStream(stream);

        // Load theme
        var theme = GetCustomTheme();

        // Setup editor
        var editorOptions = editor.Options;

    }

    private static void ConfigForConsole(TextEditor editor)
    {

    }
}