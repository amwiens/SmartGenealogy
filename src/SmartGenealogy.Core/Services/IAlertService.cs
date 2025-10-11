namespace SmartGenealogy.Core.Services;

/// <summary>
/// Alert service interface
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Show alert
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="message">Message</param>
    /// <param name="cancel">Cancel button text</param>
    Task ShowAlertAsync(string title, string message, string cancel);

    /// <summary>
    /// Show alert
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="message">Message</param>
    /// <param name="accept">Accept</param>
    /// <param name="cancel">Cancel button text</param>
    Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel);
}