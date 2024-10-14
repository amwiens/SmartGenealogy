namespace SmartGenealogy.Avalonia.Interfaces.Model;

public interface IModel
{
    /// <summary>
    /// Initializes the model.
    /// </summary>
    Task Initialize();

    /// <summary>
    /// Shutsdown the model.
    /// </summary>
    Task Shutdown();

    /// <summary>
    /// Subscribes to updates from the model.
    /// </summary>
    void SubscribeToUpdates(Action<ModelUpdateMessage> onUpdate, bool withUiDispatch = false);
}

public interface IApplicationModel
{
    /// <summary>
    /// Initializes all models.
    /// </summary>
    Task Initialize();

    /// <summary>
    /// Shutsdown all models
    /// </summary>
    Task Shutdown();
}