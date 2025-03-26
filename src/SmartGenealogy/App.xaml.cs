using SmartGenealogy.Helpers;
using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.Views;

namespace SmartGenealogy;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, "AppSettings.json")))
            SettingsManager.SaveSettings(new AppSettings());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnStart()
    {
        base.OnStart();

        var settings = SettingsManager.LoadSettings();
        if (settings.DarkMode)
            Application.Current!.Resources.ApplyDarkTheme();
        else
            Application.Current!.Resources.ApplyLightTheme();
    }
}