using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Collections;

using CommunityToolkit.Mvvm.ComponentModel;

using FluentAvalonia.UI.Controls;

using FluentIcons.Common;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.ViewModels.Base;

using Symbol = FluentIcons.Common.Symbol;
using SymbolIconSource = FluentIcons.Avalonia.Fluent.SymbolIconSource;

namespace SmartGenealogy.ViewModels.Progress;

[Singleton]
public partial class ProgressManagerViewModel : PageViewModelBase
{


    public override string Title => "Download Manager";

    public override IconSource IconSource =>
        new SymbolIconSource { Symbol = Symbol.ArrowCircleDown, IconVariant = IconVariant.Filled };

    public AvaloniaList<ProgressItemViewModelBase> ProgressItems { get; } = new();

    [ObservableProperty]
    private bool isOpen;
}