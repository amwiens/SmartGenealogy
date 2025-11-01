namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add/edit project popup view model.
/// </summary>
/// <param name="projectService">Project service</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="serviceProvider">Service provider</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class AddEditProjectPopupViewModel(
    IProjectService projectService,
    IAlertService alertService,
    IPopupService popupService,
    IServiceProvider serviceProvider,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Project? _project;

    [ObservableProperty]
    public string? _name = string.Empty;

    [ObservableProperty]
    public string? _description = string.Empty;

    [ObservableProperty]
    private int _priority = 0;

    [ObservableProperty]
    private List<string> _priorities = Enum.GetNames<Priority>().ToList();

    [ObservableProperty]
    private int _status;

    [ObservableProperty]
    private List<string> _statuses = Enum.GetNames<ProjectStatus>().ToList();

    [ObservableProperty]
    private int _category;

    [ObservableProperty]
    private List<string> _categories = EnumHelper.GetEnumDescriptionsWithBlank<ProjectCategory>();

    [ObservableProperty]
    private DateTime _startDate;

    [ObservableProperty]
    private DateTime _endDate;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int placeId = Convert.ToInt32(query["id"]);
            LoadData(placeId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Project identifier</param>
    private async Task LoadData(int id)
    {
        try
        {
            _project = await projectService.GetProjectAsync(id);

            if (_project.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Project with id {id} could not be found."));
            }

            Name = _project!.Name;
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
        if (_project is null)
            _project = new Project();

        _project.Name = Name?.Trim() ?? string.Empty;
        _project.Description = Description?.Trim() ?? string.Empty;
        _project.Priority = (Priority)Priority;
        _project.Status = (ProjectStatus)Status;
        _project.Category = (ProjectCategory)Category;
        _project.StartDate = StartDate;
        _project.EndDate = EndDate;

        var projectId = await projectService.SaveProjectAsync(_project);

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