using SmartGenealogy.Models;

namespace SmartGenealogy;

internal static class Assets
{
    public static AvaloniaResource AppIcon { get; } =
        new("avares://SmartGenealogy/Assets/Icon.ico");

    public static AvaloniaResource AppIconPng { get; } =
        new("avares://SmartGenealogy/Assets/Icon.png");

    public static AvaloniaResource ImagePromptLanguageJson =>
        new("avares://SmartGenealogy/Assets/ImagePrompt.tmLanguage.json");

    public static AvaloniaResource ThemeMatrixDarkJson =>
        new("avares://SmartGenealogy/Assets/ThemeMatrixDark.json");
}