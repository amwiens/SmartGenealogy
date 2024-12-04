using System.Linq;

using Avalonia.Controls;

using SmartGenealogy.Controls;
using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models.Update;
using SmartGenealogy.Models;
using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy.Views.Settings;

[Singleton]
public partial class UpdateSettingsPage : UserControlBase
{
    public UpdateSettingsPage()
    {
        InitializeComponent();
    }

    private void ChannelListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var listBox = (ListBox)sender!;

        if (e.AddedItems.Count == 0 || e.AddedItems[0] is not UpdateChannelCard item)
        {
            return;
        }

        var vm = (UpdateSettingsViewModel)DataContext!;

        if (!vm.VerifyChannelSelection(item))
        {
            listBox.Selection.Clear();

            listBox.Selection.SelectedItem = vm.AvailableUpdateChannelCards.First(
                c => c.UpdateChannel == UpdateChannel.Stable);

            vm.ShowLoginRequiredDialog();
        }
    }
}