using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.Views;

namespace SmartGenealogy;

public partial class App : Application
{
    public AppSettings Settings { get; set; }

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
}