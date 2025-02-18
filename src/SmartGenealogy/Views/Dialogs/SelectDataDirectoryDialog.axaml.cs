using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views.Dialogs;

[RegisterTransient<SelectDataDirectoryDialog>]
public partial class SelectDataDirectoryDialog : UserControlBase
{
    public SelectDataDirectoryDialog()
    {
        InitializeComponent();
    }
}