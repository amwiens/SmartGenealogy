using System.Globalization;

namespace SmartGenealogy.Core.Helper;

//public record struct RunningPackageStatusChangedEventArgs(PackagePair? CurrentPackagePair);

public class EventManager
{
    public static EventManager Instance { get; } = new();

    private EventManager() { }

    public event EventHandler<CultureInfo>? CultureChanged;

    public void OnCultureChanged(CultureInfo culture) => CultureChanged?.Invoke(this, culture);
}