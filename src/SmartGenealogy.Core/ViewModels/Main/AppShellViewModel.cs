namespace SmartGenealogy.Core.ViewModels.Main;

/// <summary>
/// AppShell view model.
/// </summary>
public partial class AppShellViewModel : ObservableObject, IRecipient<OpenDatabaseMessage>
{
    private readonly DatabaseSettings _databaseSettings;

    [ObservableProperty]
    private string? _title = "Smart Genealogy";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="databaseSettings">Database settings</param>
    public AppShellViewModel(DatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;

        WeakReferenceMessenger.Default.Register<OpenDatabaseMessage>(this);
    }

    /// <summary>
    /// Receives Open Database messages
    /// </summary>
    /// <param name="message">Open database message</param>
    public void Receive(OpenDatabaseMessage message)
    {
        if (message is null || message.Value == false)
        {
            Title = "Smart Genealogy";
        }
        else
        {
            Title = $"Smart Genealogy - {Path.Combine(_databaseSettings.DatabasePath!, _databaseSettings.DatabaseFilename!)}";
        }
    }
}