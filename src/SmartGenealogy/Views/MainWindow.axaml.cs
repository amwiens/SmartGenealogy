using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Avalonia.Threading;

using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Windowing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SmartGenealogy.Animations;
using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models.Settings;
using SmartGenealogy.Core.Services;
using SmartGenealogy.Models;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels;
using SmartGenealogy.ViewModels.Base;

namespace SmartGenealogy.Views;

[Singleton]
public partial class MainWindow : AppWindowBase
{
    private readonly INotificationService notificationService;
    private readonly INavigationService<MainWindowViewModel> navigationService;
    private readonly ISettingsManager settingsManager;
    private readonly ILogger<MainWindow> logger;

    private FlyoutBase? progressFlyout;

    [DesignOnly(true)]
    public MainWindow()
        : this(
              DesignData.DesignData.Services.GetRequiredService<INotificationService>(),
              DesignData.DesignData.Services.GetRequiredService<INavigationService<MainWindowViewModel>>(),
              DesignData.DesignData.Services.GetRequiredService<ISettingsManager>(),
              DesignData.DesignData.Services.GetRequiredService<ILogger<MainWindow>>())
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Design constructor called in non-design mode");
        }
    }

    public MainWindow(
        INotificationService notificationService,
        INavigationService<MainWindowViewModel> navigationService,
        ISettingsManager settingsManager,
        ILogger<MainWindow> logger)
    {
        this.notificationService = notificationService;
        this.navigationService = navigationService;
        this.settingsManager = settingsManager;
        this.logger = logger;

        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
        //this.AttachDebugSaveScreenshot();
        //LogWindow.Attach(this, App.Services);
#endif
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        navigationService.TypedNavigation += NavigationService_OnTypedNavigation;



        SetDefaultFonts();

        Observable
            .FromEventPattern<SizeChangedEventArgs>(this, nameof(SizeChanged))
            .Where(x => x.EventArgs.PreviousSize != x.EventArgs.NewSize)
            .Throttle(TimeSpan.FromMilliseconds(100))
            .Select(x => x.EventArgs.NewSize)
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(newSize =>
            {
                var validWindowPosition = Screens.All.Any(screen => screen.Bounds.Contains(Position));

                settingsManager.Transaction(
                    s =>
                    {
                        s.WindowSettings = new WindowSettings(
                            newSize.Width,
                            newSize.Height,
                            validWindowPosition ? Position.X : 0,
                            validWindowPosition ? Position.Y : 0,
                            WindowState == WindowState.Maximized);
                    },
                    ignoreMissingLibraryDir: true);
            });

        Observable
            .FromEventPattern<PixelPointEventArgs>(this, nameof(PositionChanged))
            .Where(x => Screens.All.Any(screen => screen.Bounds.Contains(x.EventArgs.Point)))
            .Throttle(TimeSpan.FromMilliseconds(100))
            .Select(x => x.EventArgs.Point)
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(position =>
            {
                settingsManager.Transaction(
                    s =>
                    {
                        s.WindowSettings = new WindowSettings(
                            Width,
                            Height,
                            position.X,
                            position.Y,
                            WindowState == WindowState.Maximized);
                    },
                    ignoreMissingLibraryDir: true);
            });
    }



    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        navigationService.SetFrame(FrameView ?? throw new NullReferenceException("Frame not found"));
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        Application.Current!.ActualThemeVariantChanged += OnActualThemeVariantChanged;

        var theme = ActualThemeVariant;
        // Enable mica for Windows 11
        if (IsWindows11 && theme != FluentAvaloniaTheme.HighContrastTheme)
        {
            TryEnableMicaEffect();
        }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
    }



    /// <inheritdoc />
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        App.Shutdown();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        // Initialize notification service using this window as the visual root
        notificationService.Initialize(this);

        // Attach error notification handler for image loader


        if (DataContext is not MainWindowViewModel vm)
            return;

        // Navigate to first page
        Dispatcher.UIThread.Post(
            () =>
            navigationService.NavigateTo(
                vm.Pages[0],
                new BetterSlideNavigationTransition
                {
                    Effect = SlideNavigationTransitionEffect.FromBottom
                }));
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
    }

    private void NavigationService_OnTypedNavigation(object? sender, TypedNavigationEventArgs e)
    {
        var mainViewModel = (MainWindowViewModel)DataContext!;

        mainViewModel.SelectedCategory = mainViewModel
            .Pages.Concat(mainViewModel.FooterPages)
            .FirstOrDefault(x => x.GetType() == e.ViewModelType);
    }



    private void SetDefaultFonts()
    {
        if (App.Current is not null)
        {
            FontFamily = App.Current.GetPlatformDefaultFontFamily();
        }
    }

    private void NavigationView_OnItemInvoked(object sender, NavigationViewItemInvokedEventArgs e)
    {
        if (e.InvokedItemContainer is NavigationViewItem nvi)
        {
            // Skip if this is the currently selected item
            if (nvi.IsSelected)
            {
                return;
            }

            if (nvi.Tag is null)
            {
                throw new InvalidOperationException("NavigationViewItem Tag is null");
            }

            if (nvi.Tag is not ViewModelBase vm)
            {
                throw new InvalidOperationException(
                    $"NavigationViewItem Tag must be of type ViewModelBase, not {nvi.Tag?.GetType()}");
            }
            navigationService.NavigateTo(vm, new BetterEntranceNavigationTransition());
        }
    }

    private void OnActualThemeVariantChanged(object? sender, EventArgs e)
    {
        if (IsWindows11)
        {
            if (ActualThemeVariant != FluentAvaloniaTheme.HighContrastTheme)
            {
                TryEnableMicaEffect();
            }
            else
            {
                ClearValue(BackgroundProperty);
                ClearValue(TransparencyBackgroundFallbackProperty);
            }
        }
    }

    private void OnImageLoadFailed(object? sender, ImageLoadFailedEventArgs e)
    {
        var fileName = Path.GetFileName(e.Url);
        var displayName = string.IsNullOrEmpty(fileName) ? e.Url : fileName;
        logger.LogWarning($"Could not load '{displayName}'\n({e.Exception.Message})");
    }

    private void TryEnableMicaEffect()
    {
        TransparencyBackgroundFallback = Brushes.Transparent;
        TransparencyLevelHint = new[]
        {
            WindowTransparencyLevel.Mica,
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Blur
        };

        if (ActualThemeVariant == ThemeVariant.Dark)
        {
            var color = this.TryFindResource("SolidBackgroundFillColorBase", ThemeVariant.Dark, out var value)
                ? (Color2)(Color)value!
                : new Color2(30, 31, 34);

            color = color.LightenPercent(-0.5f);

            Background = new ImmutableSolidColorBrush(color, 0.72);
        }
        else if (ActualThemeVariant == ThemeVariant.Light)
        {
            // Similar effect here
            var color = this.TryFindResource("SolidBackgroundFillColorBase", ThemeVariant.Dark, out var value)
                ? (Color2)(Color)value!
                : new Color2(243, 243, 243);

            color = color.LightenPercent(0.5f);

            Background = new ImmutableSolidColorBrush(color, 0.9);
        }
    }



    private void TopLevel_OnBackRequested(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        navigationService.GoBack();
    }

    private void NavigationView_OnBackRequested(object? sender, NavigationViewBackRequestedEventArgs e)
    {
        navigationService.GoBack();
    }
}