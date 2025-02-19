using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using Injectio.Attributes;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models.Settings;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Settings;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(NotificationSettingsPage))]
[RegisterSingleton<NotificationSettingsViewModel>]
[ManagedService]
public partial class NotificationSettingsViewModel(ISettingsManager settingsManager) : PageViewModelBase
{
    public override string Title => "Notifications";
    public override IconSource IconSource => new SymbolIconSource { Symbol = Symbol.Alert };

    [ObservableProperty]
    private IReadOnlyList<NotificationSettingsItem> items = [];

    public override void OnLoaded()
    {
        base.OnLoaded();

        Items = GetItems().OrderBy(item => item.Key?.Value).ToImmutableArray();
    }

    private IEnumerable<NotificationSettingsItem> GetItems()
    {
        var settingsOptions = settingsManager.Settings.NotificationOptions;

        foreach (var notificationKey in NotificationKey.All.Values)
        {
            // If in settings, include settings value, otherwise default
            if (!settingsOptions.TryGetValue(notificationKey, out var option))
            {
                option = notificationKey.DefaultOption;
            }

            yield return new NotificationSettingsItem(settingsManager)
            {
                Key = notificationKey,
                Option = option
            };
        }
    }
}