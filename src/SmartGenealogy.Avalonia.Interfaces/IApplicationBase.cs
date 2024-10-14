using SmartGenealogy.Avalonia.Interfaces.Model;

namespace SmartGenealogy.Avalonia.Interfaces;

public interface IApplicationBase
{
    IEnumerable<IModel> GetModels();

    Task Shutdown();
}
