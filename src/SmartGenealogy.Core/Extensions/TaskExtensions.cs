namespace SmartGenealogy.Core.Extensions;

/// <summary>
/// Task Extensions.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Fire and Forget Safe Async.
    /// </summary>
    /// <param name="task">Task to Fire and Forget.</param>
    /// <param name="handler">Error Handler.</param>
    public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler? handler = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            handler?.HandleError(ex);
        }
    }
}