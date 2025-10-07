namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add/edit role popup view model.
/// </summary>
/// <param name="roleRepository">Role repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Error handler</param>
public partial class AddEditRolePopupViewModel(
    RoleRepository roleRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler) : ObservableObject, IQueryAttributable
{
    private Role? _role;
    private int _factTypeId;

    [ObservableProperty]
    public string? _name = string.Empty;

    [ObservableProperty]
    public string? _sentence = string.Empty;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int roleId = Convert.ToInt32(query["id"]);
            LoadData(roleId).FireAndForgetSafeAsync();
        }
        else if (query.ContainsKey("factTypeId"))
        {
            _factTypeId = Convert.ToInt32(query["factTypeId"]);
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Role identifier</param>
    private async Task LoadData(int id)
    {
        try
        {
            _role = await roleRepository.GetAsync(id);

            if (_role.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Role with id {id} could not be found."));
            }

            Name = _role.Name;
            Sentence = _role.Sentence;
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (_role is null)
        {
            _role = new Role();
            _role.EventType = _factTypeId;
        }

        _role.Name = Name;
        _role.Sentence = Sentence;

        var roleId = await roleRepository.SaveItemAsync(_role);

        await popupService.ClosePopupAsync(Shell.Current);
    }

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await popupService.ClosePopupAsync(Shell.Current);
    }
}