using PropertyChanged;


namespace SmartGenealogy.ViewModels.Base;

[AddINotifyPropertyChangedInterface]
public partial class BaseViewModel : ObservableObject
{
    public virtual Task InitializeAsync(object navigationData)
    {
        return Task.FromResult(false);
    }

    [ObservableProperty]
    public bool isBusy = false;

    [ObservableProperty]
    public bool isRefreshing = false;

    [ObservableProperty]
    public string loadingText = string.Empty;

    [ObservableProperty]
    public bool dataLoaded = false;

    [ObservableProperty]
    public bool isErrorState = false;

    [ObservableProperty]
    public string errorMessage = string.Empty;

    [ObservableProperty]
    public string errorImage = string.Empty;

    public BaseViewModel() =>
        IsErrorState = false;

    // Set Loading Indicators for Page
    protected void SetDataLoadingIndicators(bool isStarting = true)
    {
        if (isStarting)
        {
            IsBusy = true;
            DataLoaded = false;
            IsErrorState = false;
            ErrorMessage = "";
            ErrorImage = "";
        }
        else
        {
            LoadingText = "";
            IsBusy = false;
        }
    }
}