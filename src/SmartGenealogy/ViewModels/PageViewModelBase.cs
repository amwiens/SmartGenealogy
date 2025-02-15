using FluentAvalonia.UI.Controls;

namespace SmartGenealogy.ViewModels;

public abstract class PageViewModelBase : ViewModelBase
{
    public abstract string Title { get; }
    public abstract IconSource IconSource { get; }
}