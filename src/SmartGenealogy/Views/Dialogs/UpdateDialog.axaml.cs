using Avalonia.Markup.Xaml;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.Views.Dialogs;

[Transient]
public partial class UpdateDialog : UserControlBase
{
    public UpdateDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}