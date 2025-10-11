namespace SmartGenealogy.Core.Services;

/// <summary>
/// Alert service
/// </summary>
public class AlertService : IAlertService
{
    /// <summary>
    /// Show alert
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="message">Message</param>
    /// <param name="cancel">Cancel button text</param>
    public async Task ShowAlertAsync(string title, string message, string cancel)
    {
        if (Shell.Current is Shell shell)
            await shell.DisplayAlertAsync(title, message, cancel);
    }

    /// <summary>
    /// Show alert
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="message">Message</param>
    /// <param name="accept">Accept</param>
    /// <param name="cancel">Cancel button text</param>
    public async Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel)
    {
        if (Shell.Current is Shell shell)
            return await shell.DisplayAlertAsync(title, message, accept, cancel);
        return false;
    }
}