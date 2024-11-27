using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Threading;

using AvaloniaEdit;

using Markdown.Avalonia;

using NLog;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Helpers;
using SmartGenealogy.Languages;
using SmartGenealogy.Models;

namespace SmartGenealogy;

public static class DialogHelper
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();



    /// <summary>
    /// Create a generic dialog for showing a markdown document
    /// </summary>
    public static BetterContentDialog CreateMarkdownDialog(
        string markdown,
        string? title = null,
        TextEditorPreset editorPreset = default)
    {
        Dispatcher.UIThread.VerifyAccess();

        var viewer = new MarkdownScrollViewer { Markdown = markdown };

        // Apply syntax highlighting to code blocks if preset is provided
        if (editorPreset != default)
        {
            using var _ = CodeTimer.StartDebug();

            var appliedCount = 0;

            if (
                viewer.GetLogicalDescendants().FirstOrDefault()?.GetLogicalDescendants() is
                { } stackDescendants)
            {
                foreach (var editor in stackDescendants.OfType<TextEditor>())
                {
                    TextEditorConfigs.Configure(editor, editorPreset);

                    editor.FontFamily = "Cascadia Code,Consolas,Menlo,Monospace";
                    editor.Margin = new Thickness(0);
                    editor.Padding = new Thickness(4);
                    editor.IsEnabled = false;

                    if (editor.GetLogicalParent() is Border border)
                    {
                        border.BorderThickness = new Thickness(0);
                        border.CornerRadius = new CornerRadius(4);
                    }

                    appliedCount++;
                }
            }

            Logger.Log(
                appliedCount > 0 ? LogLevel.Trace : LogLevel.Warn,
                $"Applied syntax highlighting to {appliedCount} code blocks");
        }

        return new BetterContentDialog
        {
            Title = title,
            Content = viewer,
            CloseButtonText = Resources.Action_Close,
            IsPrimaryButtonEnabled = false,
            MinDialogWidth = 800,
            MaxDialogHeight = 1000,
            MaxDialogWidth = 1000
        };
    }


}