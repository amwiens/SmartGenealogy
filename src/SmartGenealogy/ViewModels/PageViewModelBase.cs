using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAvalonia.UI.Controls;

namespace SmartGenealogy.ViewModels;

public abstract class PageViewModelBase : ViewModelBase
{
    public abstract string Title { get; }
    public abstract IconSource IconSource { get; }
}