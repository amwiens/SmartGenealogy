namespace SmartGenealogy.Models;

public interface IPersistentViewProvider
{
    Control? AttachedPersistentView { get; set; }
}