namespace SmartGenealogy.Core.ViewModels.Projects;

/// <summary>
/// Projects Dashboard Page View Model
/// </summary>
public partial class ProjectsDashboardPageViewModel(
    ProjectRepository projectRepository)
    : ObservableObject
{
    public ObservableCollection<Project> ProjectList { get; } = [];

    private List<Project> _allProjects = [];

    private Project? _draggedItem;

    private Project? _tappedItem;

    [ObservableProperty]
    private int _newProjectCount;

    [ObservableProperty]
    private int _inProgressCount;

    [ObservableProperty]
    private int _inReviewCount;

    [ObservableProperty]
    private int _doneCount;

    [ObservableProperty]
    private int _selectedOption;

    [ObservableProperty]
    private bool _isBusy;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        _allProjects = await projectRepository.ListAsync();

        AddProjectList();
    }

    /// <summary>
    /// Set counts.
    /// </summary>
    private void SetCount()
    {
        NewProjectCount = _allProjects.Count(p => p.Status == ProjectStatus.New);
        InProgressCount = _allProjects.Count(p => p.Status == ProjectStatus.InProgress);
        InReviewCount = _allProjects.Count(p => p.Status == ProjectStatus.InReview);
        DoneCount = _allProjects.Count(p => p.Status == ProjectStatus.Completed);
    }

    /// <summary>
    /// Add projects to the list based on the selected option.
    /// </summary>
    private void AddProjectList()
    {
        var filterTaskList = _allProjects.Where(f => f.Status == (ProjectStatus)SelectedOption).ToList();

        ProjectList.Clear();
        foreach (var project in filterTaskList)
        {
            ProjectList.Add(project);
        }

        SetCount();
    }

    /// <summary>
    /// Project tapped command.
    /// </summary>
    /// <param name="project">Project</param>
    [RelayCommand]
    private void ProjectTapped(Project project)
    {
        if (project == null)
            return;
        _tappedItem = project;
    }

    /// <summary>
    /// Filter the project list based on the selected option.
    /// </summary>
    /// <param name="optionStr">Option</param>
    [RelayCommand]
    private void FilterProjectList(string optionStr)
    {
        int option = Convert.ToInt32(optionStr);
        SelectedOption = -1;
        SelectedOption = option;
        AddProjectList();
    }

    /// <summary>
    /// Dragged started command.
    /// </summary>
    /// <param name="project">Project</param>
    [RelayCommand]
    private void DragStarted(Project project)
    {
        _draggedItem = project;
    }

    /// <summary>
    /// Project dropped command.
    /// </summary>
    /// <param name="optionStr"></param>
    [RelayCommand]
    private async Task ProjectDropped(string optionStr)
    {
        int option = Convert.ToInt32(optionStr);
        if (SelectedOption == option)
            return;

        IsBusy = true;
        await Task.Delay(500); // Simulate some delay for better UX

        if (_draggedItem != null)
        {
            var currentItem = _allProjects.Where(f => f.Id == _draggedItem.Id).FirstOrDefault();
            currentItem!.Status = (ProjectStatus)option;

            AddProjectList();
        }
        IsBusy = false;
    }
}