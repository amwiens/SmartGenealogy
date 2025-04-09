using CommunityToolkit.Maui.Core.Platform;

namespace SmartGenealogy.Views;

public partial class FontIconsPage : BasePage
{
    public FontIconsPage()
    {
        InitializeComponent();
        BindingContext = new FontIconsViewModel();
    }

    private void Handle_TextChanged(object sender, TextChangedEventArgs e)
    {
        var _container = BindingContext as FontIconsViewModel;
        iconsListView.BeginRefresh();

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            iconsListView.ItemsSource = _container.FontIconGroup;
        else
            iconsListView.ItemsSource = _container.FontIconGroup.Select(x =>
            new FontIconGroupModel
                (
                    x.GroupName,
                    x.Where(y => y.Name.IndexOf(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList()
                )
            ).ToList();

        iconsListView.EndRefresh();
    }

    private async void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        if (KeyboardExtensions.IsSoftKeyboardShowing(searchBar))
        {
            await KeyboardExtensions.HideKeyboardAsync(searchBar, default);
        }
        else
            await KeyboardExtensions.ShowKeyboardAsync(searchBar, default);
    }
}