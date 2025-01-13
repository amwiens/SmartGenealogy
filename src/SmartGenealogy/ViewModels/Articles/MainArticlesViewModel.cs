namespace SmartGenealogy.ViewModels.Articles;

public partial class MainArticlesViewModel : BaseViewModel
{
    public MainArticlesViewModel()
    {
        ArticleLists = new ObservableCollection<ArticleData>();
        LoadData();
    }

    void LoadData()
    {
        isBusy = true;
        Task.Run(async () =>
        {
            // await api call;
            await Task.Delay(1000);
            Application.Current!.Dispatcher.Dispatch(() =>
            {
                ArticleLists = new ObservableCollection<ArticleData>(ArticleServices.Instance.GetArticles());
                isBusy = false;
            });
        });
    }

    [ObservableProperty]
    public ObservableCollection<ArticleData> _articleLists;
}