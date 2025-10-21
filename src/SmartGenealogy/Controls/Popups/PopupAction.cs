namespace SmartGenealogy;

// All the code in this file is included in all platforms.
public class PopupAction
{
    public static async Task<T> DisplayPopup<T>(BasePopupPage page) where T : new()
    {
        try
        {
            if (Application.Current?.Windows[0].Page != null)
            {
                await Application.Current.Windows[0].Page!.Navigation.PushModalAsync(page);
            }
            return (T)await page.returnResultTask.Task;
        }
        catch (Exception ex)
        {
            return default(T)!;
        }
    }

    public static async Task<string> DisplayPopup(BasePopupPage page)
    {
        try
        {
            if (Application.Current?.Windows[0].Page != null)
                await Application.Current.Windows[0].Page!.Navigation.PushModalAsync(page);

            return (string)await page.returnResultTask.Task;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public static async Task ClosePopup(object? returnValue = null)
    {
        if (Application.Current?.Windows[0].Page != null && Application.Current.Windows[0].Page!.Navigation.ModalStack.Count > 0)
        {
            try
            {
                var currentPage = (BasePopupPage)Application.Current.Windows[0].Page!.Navigation.ModalStack.LastOrDefault()!;
                currentPage?.returnResultTask.TrySetResult(returnValue!);
            }
            catch (Exception ex)
            {
            }
            await Application.Current.Windows[0].Page!.Navigation.PopModalAsync();
        }
    }
}