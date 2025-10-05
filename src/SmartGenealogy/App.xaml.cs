

namespace SmartGenealogy;

/// <summary>
/// Application class.
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates the main window.
    /// </summary>
    /// <param name="activationState">Activation state.</param>
    /// <returns>Main window.</returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var appShell = _serviceProvider.GetService<AppShell>();
        return new Window(appShell!);
    }

    /// <summary>
    /// On start.
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();

        if (SmartGenealogySettings.OpenLastDatabaseOnStartup)
        {
            if (File.Exists(SmartGenealogySettings.LastOpenDatabase))
            {
                var fi = new FileInfo(SmartGenealogySettings.LastOpenDatabase);
                var databaseSettings = _serviceProvider.GetRequiredService<DatabaseSettings>();
                databaseSettings.DatabasePath = fi.DirectoryName;
                databaseSettings.DatabaseFilename = fi.Name;

                WeakReferenceMessenger.Default.Send(new OpenDatabaseMessage(true));
            }
        }
    }
}