using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAvalonia.UI.Controls;

using Injectio.Attributes;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Settings;

namespace SmartGenealogy.ViewModels.Settings;

[View(typeof(AISettingsPage))]
[RegisterSingleton<AISettingsViewModel>]
[ManagedService]
public partial class AISettingsViewModel(ISettingsManager settingsManager) : PageViewModelBase
{
    public override string Title => "AI Settings";
    public override IconSource IconSource => new SymbolIconSource { Symbol = Symbol.Alert };
}