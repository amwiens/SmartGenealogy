﻿using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Abstractions;

using SmartGenealogy.Core.Services;

namespace SmartGenealogy.DesignData;

public class MockSettingsManager() : SettingsManager(NullLogger<SettingsManager>.Instance)
{
    protected override void LoadSettings(CancellationToken cancellationToken = default) { }

    protected override Task LoadSettingsAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected override void SaveSettings(CancellationToken cancellationToken = default) { }

    protected override Task SaveSettingsAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}