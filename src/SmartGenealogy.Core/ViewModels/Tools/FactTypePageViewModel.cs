using SmartGenealogy.Core.Services;

namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Fact type page view model.
/// </summary>
/// <param name="factTypeService">Fact type service</param>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="roleRepository">Role repository</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class FactTypePageViewModel(
    IFactTypeService factTypeService,
    FactTypeRepository factTypeRepository,
    RoleRepository roleRepository,
    IAlertService alertService,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private FactType? _factType;

    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private string? _abbreviation = string.Empty;

    [ObservableProperty]
    private string? _gedcomTag = string.Empty;

    [ObservableProperty]
    private bool _useValue = false;

    [ObservableProperty]
    private bool _useDate = false;

    [ObservableProperty]
    private bool _usePlace = false;

    [ObservableProperty]
    private string? _sentence = string.Empty;

    [ObservableProperty]
    private bool _isBuiltIn = false;

    [ObservableProperty]
    private ObservableCollection<Role> _roles = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditRoleCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteRoleCommand))]
    private Role? _selectedRole;

    private bool CanEditRole() => SelectedRole != null && SelectedRole.Id != 0;

    private bool CanDeleteRole() => SelectedRole != null && SelectedRole.Id != 0;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int factTypeId = Convert.ToInt32(query["id"]);
            LoadData(factTypeId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Record identifier</param>
    /// <returns></returns>
    private async Task LoadData(int id)
    {
        try
        {
            _factType = await factTypeRepository.GetAsync(id);

            if (_factType.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
                return;
            }

            Name = _factType.Name;
            Abbreviation = _factType.Abbreviation;
            GedcomTag = _factType.GedcomTag;
            UseValue = _factType.UseValue;
            UseDate = _factType.UseDate;
            UsePlace = _factType.UsePlace;
            Sentence = _factType.Sentence;
            IsBuiltIn = _factType.IsBuiltIn;
            Roles = new ObservableCollection<Role>(_factType.Roles);
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Edit fact type.
    /// </summary>
    [RelayCommand]
    private async Task EditFactType()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _factType!.Id }
        };

        await popupService.ShowPopupAsync<AddEditFactTypePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete fact type.
    /// </summary>
    [RelayCommand]
    private async Task DeleteFactType()
    {
        try
        {
            var isConfirmed = await alertService.ShowAlertAsync("Delete fact type", "Are you sure you want to delete this fact type?", "Yes", "No");
            if (isConfirmed)
            {
                var isDeleted = await factTypeService.DeleteItemAsync(_factType!);
                if (isDeleted)
                    await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Add role
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AddRole()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "factTypeId", _factType!.Id }
        };

        await popupService.ShowPopupAsync<AddEditRolePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Edit role
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditRole))]
    private async Task EditRole()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", SelectedRole!.Id }
        };

        await popupService.ShowPopupAsync<AddEditRolePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete role
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteRole))]
    private async Task DeleteRole()
    {
        try
        {
            var isConfirmed = await alertService.ShowAlertAsync("Delete role", "Are you sure you want to delete this role?", "Yes", "No");
            if (isConfirmed)
            {
                await roleRepository.DeleteItemAsync(SelectedRole!);
                LoadData(_factType!.Id).FireAndForgetSafeAsync();
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }
}