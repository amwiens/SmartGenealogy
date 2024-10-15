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

    // Logger will never be null or else
}