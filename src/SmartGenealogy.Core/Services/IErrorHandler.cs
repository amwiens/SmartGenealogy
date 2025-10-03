namespace SmartGenealogy.Core.Services;

/// <summary>
/// Error Handler Service.
/// </summary>
public interface IErrorHandler
{
    /// <summary>
    /// handle error in UI.
    /// </summary>
    /// <param name="ex">Exception being thrown.</param>
    void HandleError(Exception ex);
}