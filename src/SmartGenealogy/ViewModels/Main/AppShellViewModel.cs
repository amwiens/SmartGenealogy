namespace SmartGenealogy.ViewModels.Main;

public partial class AppShellViewModel : ObservableObject, IRecipient<DatabaseOpenMessage>
{
    [ObservableProperty]
    private string _title = "Smart Genealogy";

    public AppShellViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DatabaseOpenMessage message)
    {
        Title = Title + message.Value;
    }
}