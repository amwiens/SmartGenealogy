using Avalonia.Markup.Xaml;

using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.Controls;

[Transient]
public partial class RefreshBadge : UserControlBase
{
    public RefreshBadge()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}