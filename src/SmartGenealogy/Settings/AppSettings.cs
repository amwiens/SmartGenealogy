namespace SmartGenealogy.Settings;

public class AppSettings
{
    public static string ImageServerPath { get; } = "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/";

    public static bool IsFirstLaunching
    {
        get => Preferences.Get(nameof(IsFirstLaunching), true);
        set => Preferences.Set(nameof(IsFirstLaunching), value);
    }

    // http://www.lingoes.net/en/translator/langcode.htm
    public static List<LanguageSelectItem> Languages = new List<LanguageSelectItem>
    {
        new LanguageSelectItem { Code ="en-US", Flag="flag_en.png", Name = "English (United State)", IsRTL = false },
        new LanguageSelectItem { Code ="ar-AE", Flag="flag_ar.png", Name = "Arabic (U.A.E.)", IsRTL = true },
        new LanguageSelectItem { Code ="de-DE", Flag="flag_de.png", Name = "German (Germany)", IsRTL = false },
        new LanguageSelectItem { Code ="es-ES", Flag="flag_es.png", Name = "Spanish (Spain)", IsRTL = false },
        new LanguageSelectItem { Code ="fr-FR", Flag="flag_fr.png", Name = "French (France)", IsRTL = false },
        new LanguageSelectItem { Code ="nl-NL", Flag="flag_nl.png", Name = "Dutch (Netherlands)", IsRTL = false },
        new LanguageSelectItem { Code ="pt-PT", Flag="flag_pt.png", Name = "Portuguese (Portugal)", IsRTL = false },
    };

    public static string DefaultLanguageCode = "en-US";

    public static LanguageSelectItem SelectedLanguageItem
    {
        get => PreferencesHelpers.Get(nameof(SelectedLanguageItem), default(LanguageSelectItem));
        set => PreferencesHelpers.Set(nameof(SelectedLanguageItem), value);
    }

    public static string LanguageCodeSelected
    {
        get => Preferences.Get(nameof(LanguageCodeSelected), DefaultLanguageCode);
        set => Preferences.Set(nameof(LanguageCodeSelected), value);
    }

    public static bool IsRTLLanguage
    {
        get => Preferences.Get(nameof(IsRTLLanguage), false);
        set => Preferences.Set(nameof(IsRTLLanguage), value);
    }

    public static bool IsDarkMode
    {
        get => Preferences.Get(nameof(IsDarkMode), false);
        set => Preferences.Set(nameof(IsDarkMode), value);
    }

    public static bool IsUseSystemTheme
    {
        get => Preferences.Get(nameof(IsUseSystemTheme), false);
        set => Preferences.Set(nameof(IsUseSystemTheme), value);
    }

    public static int SelectedPrimaryColorIndex
    {
        get => Preferences.Get(nameof(SelectedPrimaryColorIndex), 0);
        set => Preferences.Set(nameof(SelectedPrimaryColorIndex), value);
    }

    public static PrimaryColorItem SelectedPrimaryColorCollectionItem
    {
        get => PreferencesHelpers.Get(nameof(SelectedPrimaryColorCollectionItem), default(PrimaryColorItem));
        set => PreferencesHelpers.Set(nameof(SelectedPrimaryColorCollectionItem), value);
    }
}