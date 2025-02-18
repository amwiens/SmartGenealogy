using Injectio.Attributes;

namespace SmartGenealogy.Controls;

[RegisterTransient<RefreshBadge>]
public partial class RefreshBadge : UserControlBase
{
    public RefreshBadge()
    {
        InitializeComponent();
    }
}