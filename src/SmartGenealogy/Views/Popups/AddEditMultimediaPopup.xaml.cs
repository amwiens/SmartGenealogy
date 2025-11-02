#if IOS || MACCATALYST
using Foundation;
using UIKit;
#endif

#if WINDOWS
using StandardDataFormats = Windows.ApplicationModel.DataTransfer.StandardDataFormats;
using Windows.Storage;
#endif

namespace SmartGenealogy.Views.Popups;

/// <summary>
/// Add multimedia popup
/// </summary>
public partial class AddEditMultimediaPopup : Popup<int>
{
    private readonly AddEditMultimediaPopupViewModel _viewModel;

    /// <summary>
    /// Constructor
    /// </summary>
    public AddEditMultimediaPopup(AddEditMultimediaPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    /// <summary>
    /// Image drop
    /// </summary>
    private async void OnImageDrop(object sender, DropEventArgs e)
    {
        var filePaths = new List<string>();

#if WINDOWS
        if (e.PlatformArgs is not null && e.PlatformArgs.DragEventArgs.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await e.PlatformArgs.DragEventArgs.DataView.GetStorageItemsAsync();
            if (items.Any())
            {
                foreach (var item in items)
                {
                    if (item is StorageFile file)
                        filePaths.Add(item.Path);
                }
            }
        }
#elif IOS || MACCATALYST
        var session = e.PlatformArgs?.DropSession;
        foreach (UIDragItem item in session!.Items)
        {
            var result = await LoadItemAsync(item.ItemProvider, item.ItemProvider.RegisteredTypeIdentifiers.ToList());
            if (result is not null)
                filePaths.Add(result.FileUrl?.Path!);
        }

        static async Task<LoadInPlaceResult> LoadItemAsync(NSItemProvider itemProvider, List<string> typeIdentifiers)
        {
            if (typeIdentifiers is null || typeIdentifiers.Count == 0)
                return null;

            var typeIdent = typeIdentifiers.First();

            if (itemProvider.HasItemConformingTo(typeIdent))
                return await itemProvider.LoadInPlaceFileRepresentationAsync(typeIdent);

            typeIdentifiers.Remove(typeIdent);
            return await LoadItemAsync(itemProvider, typeIdentifiers);
        }
#endif

        var filePath = filePaths.FirstOrDefault();
        await _viewModel.DropMediaCommand.ExecuteAsync(filePath);
    }
}