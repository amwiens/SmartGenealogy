using System.Collections.ObjectModel;

namespace SmartGenealogy.ViewModels.Main;

public partial class AppShellViewModel : ObservableObject, IRecipient<DatabaseOpenMessage>
{
    [ObservableProperty]
    private string? _databasePath = string.Empty;

    public ObservableCollection<FlyoutMenuItem> MenuItems { get; } = new()
    {
        new FlyoutMenuItem { Title = LocalizationResourceManager.Translate("MenuHome"), FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Home }, Route = "main" },
        new FlyoutMenuItem { Title = LocalizationResourceManager.Translate("MenuMedia"), FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Image }, Route = "media" },
        new FlyoutMenuItem { Title = LocalizationResourceManager.Translate("MenuTools"), FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Tools }, Route = "tools" },
        new FlyoutMenuItem { Title = LocalizationResourceManager.Translate("MenuSettings"), FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Cog }, Route = "settings" }
    };

    public AppShellViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DatabaseOpenMessage message)
    {
        DatabasePath = message.Value;
    }
}