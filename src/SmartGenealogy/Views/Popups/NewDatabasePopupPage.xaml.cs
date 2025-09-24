namespace SmartGenealogy.Views.Popups;

public partial class NewDatabasePopupPage : PopupPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseSettings _databaseSettings;

	public NewDatabasePopupPage(IServiceProvider serviceProvider, DatabaseSettings databaseSettings)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        _databaseSettings = databaseSettings;
	}

    private async void SelectFolderButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FolderPicker.Default.PickAsync();
            if (result != null)
            {
                databasePath.Text = result.Folder!.Path;
            }
        }
        catch { }
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        var name = $"{databaseName.Text.Trim()}.sgdb";
        var path = databasePath.Text.Trim();

        _databaseSettings.DatabaseName = name;
        _databaseSettings.DatabasePath = path;

        var databaseInitializer = _serviceProvider.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.LoadSeedDataAsync();

        WeakReferenceMessenger.Default.Send(new DatabaseOpenMessage(Path.Combine(path, name)));
        await PopupNavigation.Instance.PopAsync();
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }
}