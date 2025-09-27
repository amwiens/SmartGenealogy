namespace SmartGenealogy.ViewModels.Media;

public partial class MediaDetailsPageViewModel : ObservableObject, IQueryAttributable
{
    private Multimedia? _multimedia;
    private readonly MultimediaRepository _multimediaRepository;
    private readonly IPopupService _popupService;
    private readonly ModalErrorHandler _errorHandler;

    [ObservableProperty]
    bool _isBusy;

    [ObservableProperty]
    private string? _filePath;

    [ObservableProperty]
    private string? _caption;

    [ObservableProperty]
    private string? _refNumber;

    [ObservableProperty]
    private string? _date;

    [ObservableProperty]
    private string? _sortDate;

    [ObservableProperty]
    private string? _description;

    public MediaDetailsPageViewModel(MultimediaRepository multimediaRepository, IPopupService popupService)
    {
        _multimediaRepository = multimediaRepository;
        _popupService = popupService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int multimediaId = Convert.ToInt32(query["id"]);
            LoadData(multimediaId).FireAndForgetSafeAsync();
        }
    }

    private async Task RefreshData()
    {
        if (_multimedia!.IsNullOrNew())
        {
            return;
        }

        await LoadData(_multimedia.Id);
    }

    private async Task LoadData(long id)
    {
        try
        {
            IsBusy = true;

            _multimedia = await _multimediaRepository.GetAsync(id);

            if (_multimedia!.IsNullOrNew())
            {
                _errorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
                return;
            }

            FilePath = _multimedia.FullPath;
            Caption = _multimedia.Caption;
            RefNumber = _multimedia.RefNumber;
            Date = _multimedia.Date;
            SortDate = _multimedia.SortDate.ToString();
            Description = _multimedia.Description;
        }
        catch (Exception ex)
        {
            _errorHandler.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditFactType()
    {

    }

    [RelayCommand]
    private async Task DeleteFactType()
    {
        await _multimediaRepository.DeleteItemAsync(_multimedia!.Id);
        await Shell.Current.GoToAsync("..");
    }
}