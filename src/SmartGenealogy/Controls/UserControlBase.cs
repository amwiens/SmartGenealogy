using AsyncAwaitBestPractices;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

using SmartGenealogy.ViewModels.Base;

namespace SmartGenealogy.Controls;

public class UserControlBase : UserControl
{
    static UserControlBase()
    {
        LoadedEvent.AddClassHandler<UserControlBase>((cls, args) => cls.OnLoadedEvent(args));

        UnloadedEvent.AddClassHandler<UserControlBase>((cls, args) => cls.OnUnloadedEvent(args));
    }

    protected virtual void OnLoadedEvent(RoutedEventArgs? e)
    {
        if (DataContext is not ViewModelBase viewModel)
            return;

        // Run synchronous load then async load
        viewModel.OnLoaded();

        // Can't block here so we'll run as async on UI thread
        Dispatcher.UIThread.InvokeAsync(viewModel.OnLoadedAsync).SafeFireAndForget();
    }

    protected virtual void OnUnloadedEvent(RoutedEventArgs? e)
    {
        if (DataContext is not ViewModelBase viewModel)
            return;

        // Run synchronous unload then async unload
        viewModel.OnUnloaded();

        // Can't block here so we'll run as async on UI thread
        Dispatcher.UIThread.InvokeAsync(viewModel.OnUnloadedAsync).SafeFireAndForget();
    }
}