namespace SmartGenealogy.Views.Popups;

/// <summary>
/// New database popup.
/// </summary>
public partial class NewDatabasePopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">New database popup view model</param>
    public NewDatabasePopup(NewDatabasePopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}