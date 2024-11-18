using System;
using System.Linq;

using Avalonia.Controls;

using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;

using SmartGenealogy.Animations;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Models;
using SmartGenealogy.ViewModels;
using SmartGenealogy.ViewModels.Base;

namespace SmartGenealogy.Services;

[Singleton(ImplType = typeof(NavigationService<MainWindowViewModel>),
    InterfaceType = typeof(INavigationService<MainWindowViewModel>))]
//[Singleton(ImplType = typeof(NavigationService<SettingsViewModel>),
//    InterfaceType = typeof(INavigationService<SettingsViewModel>))]
public class NavigationService<T> : INavigationService<T>
{
    private Frame? frame;

    public event EventHandler<TypedNavigationEventArgs>? TypedNavigation;

    /// <inheritdoc />
    public void SetFrame(Frame frame)
    {
        this.frame = frame;
    }

    /// <inheritdoc />
    public void NavigateTo<TViewModel>(NavigationTransitionInfo? transitionInfo = null, object? param = null)
        where TViewModel : ViewModelBase
    {
        if (frame is null)
        {
            throw new InvalidOperationException("SetFrame was not called before NavigateTo.");
        }

        if (App.Services.GetService(typeof(ISettingsManager)) is ISettingsManager settingsManager)
        {
            // Handle animation scale
            switch (transitionInfo)
            {
                // If the transition info is null or animation scale is 0, suppress the transition
                case null:
                case BaseTransitionInfo when settingsManager.Settings.AnimationScale == 0f:
                    transitionInfo = new SuppressNavigationTransitionInfo();
                    break;

                case BaseTransitionInfo baseTransitionInfo:
                    baseTransitionInfo.Duration *= settingsManager.Settings.AnimationScale;
                    break;
            }
        }

        frame.NavigateToType(
            typeof(TViewModel),
            param,
            new FrameNavigationOptions
            {
                IsNavigationStackEnabled = true,
                TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
            });

        TypedNavigation?.Invoke(this, new TypedNavigationEventArgs { ViewModelType = typeof(TViewModel) });
    }

    /// <inheritdoc />
    public void NavigateTo(
        Type viewModelType,
        NavigationTransitionInfo? transitionInfo = null,
        object? param = null)
    {
        if (!viewModelType.IsAssignableTo(typeof(ViewModelBase)))
        {
            throw new ArgumentException("Type must be a ViewModelBase.", nameof(viewModelType));
        }

        if (frame is null)
        {
            throw new InvalidOperationException("SetFrame was not called before NavigateTo.");
        }

        if (App.Services.GetService(typeof(ISettingsManager)) is ISettingsManager settingsManager)
        {
            // Handle animation scale
            switch (transitionInfo)
            {
                // If the transition info is null or animation scale is 0, suppress the transition
                case null:
                case BaseTransitionInfo when settingsManager.Settings.AnimationScale == 0f:
                    transitionInfo = new SuppressNavigationTransitionInfo();
                    break;

                case BaseTransitionInfo baseTransitionInfo:
                    baseTransitionInfo.Duration *= settingsManager.Settings.AnimationScale;
                    break;
            }
        }

        frame.NavigateToType(
            viewModelType,
            param,
            new FrameNavigationOptions
            {
                IsNavigationStackEnabled = true,
                TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
            });

        TypedNavigation?.Invoke(this, new TypedNavigationEventArgs { ViewModelType = viewModelType });
    }

    /// <inheritdoc />
    public void NavigateTo(ViewModelBase viewModel, NavigationTransitionInfo? transitionInfo = null)
    {
        if (frame is null)
        {
            throw new InvalidOperationException("SetFrame was not called before NavigateTo.");
        }

        if (App.Services.GetService(typeof(ISettingsManager)) is ISettingsManager settingsManager)
        {
            // Handle animation scale
            switch (transitionInfo)
            {
                // If the transition info is null or animation scale is 0, suppress the transition
                case null:
                case BaseTransitionInfo when settingsManager.Settings.AnimationScale == 0f:
                    transitionInfo = new SuppressNavigationTransitionInfo();
                    break;

                case BaseTransitionInfo baseTransitionInfo:
                    baseTransitionInfo.Duration *= settingsManager.Settings.AnimationScale;
                    break;
            }
        }

        frame.NavigateFromObject(
            viewModel,
            new FrameNavigationOptions
            {
                IsNavigationStackEnabled = true,
                TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
            });

        TypedNavigation?.Invoke(
            this,
            new TypedNavigationEventArgs { ViewModelType = viewModel.GetType(), ViewModel = viewModel });
    }

    public bool GoBack()
    {
        if (frame?.Content is IHandleNavigation navigationHandler)
        {
            var wentBack = navigationHandler.GoBack();
            if (wentBack)
            {
                return true;
            }
        }

        if (frame is not { CanGoBack: true })
            return false;

        TypedNavigation?.Invoke(
            this,
            new TypedNavigationEventArgs
            {
                ViewModelType = frame.BackStack.Last().SourcePageType,
                ViewModel = frame.BackStack.Last().Context
            });

        frame.GoBack();
        return true;
    }

    public bool CanGoBack => frame?.CanGoBack ?? false;

    public object? CurrentPageDataContext => (frame?.Content as Control)?.DataContext;
}