using System.Collections.ObjectModel;

namespace SmartGenealogy.ViewModels.Main;

public partial class AppShellViewModel : ObservableObject, IRecipient<DatabaseOpenMessage>
{
    [ObservableProperty]
    private string _title = "Smart Genealogy";

    public ObservableCollection<FlyoutMenuItem> MenuItems { get; } = new()
    {
        new FlyoutMenuItem { Title = "Home", FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Home }, Route = "main" },
        new FlyoutMenuItem { Title = "Media", FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Image }, Route = "media" },
        new FlyoutMenuItem { Title = "Settings", FontImageSource = new FontImageSource { Color = Colors.White, FontFamily = "FaPro", Glyph = FaPro.Cog }, Route = "settings" }
    };

    public AppShellViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DatabaseOpenMessage message)
    {
        Title = Title + message.Value;
    }
}