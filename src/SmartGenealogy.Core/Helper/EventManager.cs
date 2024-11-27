using System.Globalization;

using SmartGenealogy.Core.Models.Update;

namespace SmartGenealogy.Core.Helper;

public class EventManager
{
    public static EventManager Instance { get; } = new();

    private EventManager() { }


    public event EventHandler<UpdateInfo>? UpdateAvailable;

    public EventHandler<CultureInfo>? CultureChanged;



    public void OnUpdateAvailable(UpdateInfo args) => UpdateAvailable?.Invoke(this, args);



    public void OnCultureChanged(CultureInfo culture) => CultureChanged?.Invoke(this, culture);
}