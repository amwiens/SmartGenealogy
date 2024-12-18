﻿using System;
using System.Linq;
using System.Threading.Tasks;

using AsyncAwaitBestPractices;

using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Helper.HardwareInfo;
using SmartGenealogy.Languages;
using SmartGenealogy.Styles;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views;

namespace SmartGenealogy.ViewModels;

[View(typeof(FirstLaunchSetupWindow))]
[ManagedService]
[Singleton]
public partial class FirstLaunchSetupViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool eulaAccepted;

    [ObservableProperty]
    private string gpuInfoText = string.Empty;

    [ObservableProperty]
    private RefreshBadgeViewModel checkHardwareBadge =
        new()
        {
            WorkingToolTipText = Resources.Label_CheckingHardware,
            SuccessToolTipText = Resources.Label_EverythingLooksGood,
            FailToolTipText = Resources.Label_NvidiaGpuRecommended,
            FailColorBrush = ThemeColors.ThemeYellow,
        };

    public FirstLaunchSetupViewModel()
    {
        CheckHardwareBadge.RefreshFunc = SetGpuInfo;
    }

    private async Task<bool> SetGpuInfo()
    {
        GpuInfo[] gpuInfo;

        await using (new MinimumDelay(800, 1200))
        {
            // Query GPU info
            gpuInfo = await Task.Run(() => HardwareHelper.IterGpuInfo().ToArray());
        }

        // First Nvidia GPU
        var activeGpu = gpuInfo.FirstOrDefault(
            gpu => gpu.Name?.Contains("nvidia", StringComparison.InvariantCultureIgnoreCase) ?? false);
        var isNvidia = activeGpu is not null;

        // Otherwise first GPU
        activeGpu ??= gpuInfo.FirstOrDefault();

        GpuInfoText = activeGpu is null
            ? "No GPU detected"
            : $"{activeGpu.Name} ({Size.FormatBytes(activeGpu.MemoryBytes)})";

        // Always succeed for macos arm
        if (Compat.IsMacOS && Compat.IsArm)
        {
            return true;
        }

        return isNvidia;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        CheckHardwareBadge.RefreshCommand.ExecuteAsync(null).SafeFireAndForget();
    }
}