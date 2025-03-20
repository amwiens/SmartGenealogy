using SmartGenealogy.Resources.Themes;

namespace SmartGenealogy.Helpers;

public static class ThemeUtil
{
    public static void ApplyDarkTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current!.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var lightTheme = mergedDictionaries.OfType<LightTheme>().FirstOrDefault();
                if (lightTheme != null)
                {
                    mergedDictionaries.Remove(lightTheme);
                }
                mergedDictionaries.Add(new DarkTheme());
            }
        }
    }

    public static void ApplyLightTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current!.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var darkTheme = mergedDictionaries.OfType<DarkTheme>().FirstOrDefault();
                if (darkTheme != null)
                {
                    mergedDictionaries.Remove(darkTheme);
                }
                mergedDictionaries.Add(new LightTheme());
            }
        }
    }
}