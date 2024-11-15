using System.Globalization;

namespace SmartGenealogy.Core.Helper;

public class EventManager
{
    public static EventManager Instance { get; } = new();

    private EventManager() { }


    public EventHandler<CultureInfo>? CultureChanged;



    public void OnCultureChanged(CultureInfo culture) => CultureChanged?.Invoke(this, culture);
}