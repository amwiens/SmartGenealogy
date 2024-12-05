using Avalonia.Markup.Xaml;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.Views.Dialogs;

[Transient]
public partial class CreateProjectDialog : UserControlBase
{
    public CreateProjectDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}