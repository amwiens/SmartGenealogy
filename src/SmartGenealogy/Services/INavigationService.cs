using FluentAvalonia.UI.Media.Animation;

namespace SmartGenealogy.Services;

public interface INavigationService<T>
{
    event EventHandler<TypedNavigationEventArgs>? TypedNavigation;

    /// <summary>
    /// Set the frame to use for navigation.
    /// </summary>
    void SetFrame(Frame frame);

    /// <summary>
    /// Navigate to the view of the given view model type.
    /// </summary>
    void NavigateTo<TViewModel>(NavigationTransitionInfo? transitionInfo = null, object? param = null)
        where TViewModel : ViewModelBase;

    /// <summary>
    /// Navigate to the view of the given view model type.
    /// </summary>
    void NavigateTo(
        Type viewModelType,
        NavigationTransitionInfo? transitionInfo = null,
        object? param = null);

    /// <summary>
    /// Navigate to the view of the given view model.
    /// </summary>
    void NavigateTo(ViewModelBase viewModel, NavigationTransitionInfo? transitionInfo = null);

    bool GoBack();

    bool CanGoBack { get; }
    object? CurrentPageDataContext { get; }
}