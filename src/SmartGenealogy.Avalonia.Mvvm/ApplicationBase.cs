using System.IO.Pipes;

namespace SmartGenealogy.Avalonia.Mvvm;

public class ApplicationBase(
    string organizationKey,
    string applicationKey,
    string uriString,
    Type mainWindowType,
    Type applicationModelType,
    List<Type> modelTypes,
    List<Type> singletonTypes,
    List<Tuple<Type, Type>> servicesInterfaceAndType,
    bool singleInstanceRequested = false) : Application, IApplicationBase
{
    public static Window MainWindow;

    // The host cannot be null or else there is no app...
    public static IHost AppHost { get; private set; }

    // Logger will never be null or else the app did not take off
    public ILogger Logger { get; private set; }

    // LATER, maybe, using Fluent theme for now
    //public StyleManager StyleManager { get; private set; }

    // To enforce single instance
    private static FileStream? LockFile;

    private readonly string organizationKey = organizationKey;
    private readonly string applicationKey = applicationKey;
    // We may need this one later
    private readonly string uriString = uriString;
    private readonly Type mainWindowType = mainWindowType;
    private readonly Type applicationModelType = applicationModelType;
    private readonly List<Type> modelTypes = modelTypes;
    private readonly List<Type> singletonTypes = singletonTypes;
    private readonly List<Tuple<Type, Type>> servicesInterfaceAndType = servicesInterfaceAndType;
    private readonly List<Type> validatedModelTypes = [];
    private readonly bool isSingleInstanceRequested = singleInstanceRequested;

    private IClassicDesktopStyleApplicationLifetime? desktop;

    public override async void OnFrameworkInitializationCompleted()
    {
        // Try to catch all exceptions, missing the ones on the main thread at this time
        TaskScheduler.UnobservedTaskException += this.OnTaskSchedulerUnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;

        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            this.desktop = lifetime;
        }

        // Enforce single instance if requested
        if (this.isSingleInstanceRequested && this.IsAlreadyRunning() && (this.desktop is not null))
        {
            this.ForceShutdown();
            return;
        }

        this.InitializeHosting();

        if (Design.IsDesignMode)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (this.desktop is not null)
        {
            var startupWindow = ApplicationBase.GetRequiredService<Window>();
            if (startupWindow is Window window)
            {
                ApplicationBase.MainWindow = window;
                this.desktop.MainWindow = window;
                // LATER, maybe, using Fluent theme for now
                //this.StyleManager = new StyleManager(window);
            }
            else
            {
                throw new NotImplementedException("Failed to create MainWindow");
            }
        }
        else
        {
            // Should not be in designer mode
            throw new NotImplementedException("Unsupported Application Lifetime");
        }

        base.OnFrameworkInitializationCompleted();
        await this.Startup();
    }

    private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        this.GlobalExceptionHandler(exception);
    }

    private void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        var exception = e.Exception;
        this.GlobalExceptionHandler(exception);
    }

    private void InitializeHosting()
    {
        ApplicationBase.AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_0, services) =>
            {
                // Register the app
                _ = services.AddSingleton(typeof(IApplicationBase), this);

                // Always Main Window
                _ = services.AddSingleton(typeof(Window), this.mainWindowType);

                // The Application Model, also a singleton, no need here to also add it without the interface
                _ = services.AddSingleton(typeof(IApplicationModel), this.applicationModelType);

                // Models
                foreach (Type modelType in this.modelTypes)
                {
                    bool isModel = typeof(IModel).IsAssignableFrom(modelType);
                    if (isModel)
                    {
                        // Models can be retrieved all via the interface or retrieved only one by providing specific type,
                        // just like singletons below
                        _ = services.AddSingleton(modelType);
                        this.validatedModelTypes.Add(modelType);
                    }
                    else
                    {
                        Debug.WriteLine(modelType.FullName!.ToString() + " is not a IModel");
                    }
                }

                // Singletons, they do not need an interface.
                foreach (var singletonType in this.singletonTypes)
                {
                    _ = services.AddSingleton(singletonType);
                }

                // Services, all must comply to a specific interface
                foreach (var serviceType in this.servicesInterfaceAndType)
                {
                    var interfaceType = serviceType.Item1;
                    var implementationType = serviceType.Item2;
                    _ = services.AddSingleton(interfaceType, implementationType);
                }
            }).Build();
    }

    public static T GetRequiredService<T>() where T : notnull
        => ApplicationBase.AppHost!.Services.GetRequiredService<T>();

    public static object GetRequiredService(Type type)
        => ApplicationBase.AppHost!.Services.GetRequiredService(type);

    public static TModel GetModel<TModel>() where TModel : notnull
    {
        TModel? model = ApplicationBase.GetRequiredService<TModel>() ??
            throw new ApplicationException("No model of type " + typeof(TModel).FullName);
        bool isModel = typeof(IModel).IsAssignableFrom(typeof(TModel));
        if (!isModel)
        {
            throw new ApplicationException(typeof(TModel).FullName + " is not a IModel");
        }

        return model;
    }


}