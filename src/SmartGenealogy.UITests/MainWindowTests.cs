﻿using Avalonia.Controls;
using Avalonia.VisualTree;

using FluentAvalonia.UI.Controls;

using Microsoft.Extensions.DependencyInjection;

using SmartGenealogy.UITests.Extensions;
using SmartGenealogy.ViewModels;

namespace SmartGenealogy.UITests;

[Collection("TempDir")]
[TestCaseOrderer("SmartGenealogy.UITests.PriorityOrderer", "SmartGenealogy.UITests")]
public class MainWindowTests : TestBase
{
    [AvaloniaFact, TestPriority(1)]
    public async Task MainWindow_ShouldOpen()
    {
        var (window, _) = GetMainWindow();

        window.Show();
        await Task.Delay(300);

        await DoInitialSetup();

        await Task.Delay(1000);
        await Verify(window, Settings);
    }

    [AvaloniaFact, TestPriority(2)]
    public async Task MainWindowViewModel_ShouldOk()
    {
        var viewModel = Services.GetRequiredService<MainWindowViewModel>();
        await Verify(viewModel, Settings);
    }

    [AvaloniaFact, TestPriority(3)]
    public async Task NavigateToModelBrowser_ShouldWork()
    {
        //var (window, viewModel) = GetMainWindow();
        //await DoInitialSetup();

        //var y = window
        //    .FindDescendantOfType<NavigationView>()!
        //    .GetVisualDescendants()
        //    .OfType<NavigationViewItem>()
        //    .FirstOrDefault(i => i.Content?.ToString() == "")!;

        //await window.ClickTargetAsync(y);

        //var frame = window.FindControl<Frame>("FrameView");
        //Assert.IsType<CheckpointBrowserPage>(frame!.Content);

        //await Task.Delay(1000);
        //SaveScreenshot(window);
        //await Verify(window, Settings);
    }
}