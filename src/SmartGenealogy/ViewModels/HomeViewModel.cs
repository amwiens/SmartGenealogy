using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Languages;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels;

[View(typeof(HomePage))]
[Singleton]
public partial class HomeViewModel : PageViewModelBase
{
    public override string Title => Resources.Label_Home;
    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.Home, IconVariant = IconVariant.Filled };
}