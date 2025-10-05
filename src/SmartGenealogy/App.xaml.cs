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
}