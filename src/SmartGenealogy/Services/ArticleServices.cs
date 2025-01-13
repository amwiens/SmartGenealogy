namespace SmartGenealogy.Services;

public class ArticleServices
{
    static ArticleServices _instance;

    public static ArticleServices Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ArticleServices();

            return _instance;
        }
    }

    public ArticleData GetArticle(int articleId)
    {
        var article = GetArticles().Where(x => x.Id == articleId).FirstOrDefault();
        return article;
    }

    public List<ArticleData> GetArticles()
    {
        return new List<ArticleData>
            {
                new ArticleData
                {
                    Id = 001,
                    Title =  "Basic Inner Workings of Camera",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "ACTUALITY",
                    SectionColor = Color.FromRgba("#ff2602"),
                    Author =  "TLS SOFTWARE",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a1.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_01.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/WeAreGoingOnBullrun.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "John Doe",
                    When =  "JAN 6, 2017",
                    Followers =  "112",
                },
                new ArticleData
                {
                    Id = 002,
                    Title =  "Vintage Beauty in Decorations",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "SPORTS",
                    SectionColor = Color.FromRgba("#ffc000"),
                    Author =  "Julia Grant",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a2.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_02.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/WhatCarCanYouGetForAGrand.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Julia Grant",
                    When =  "FEB 13, 2017",
                    Followers =  "340",
                },
                new ArticleData
                {
                    Id = 003,
                    Title =  "Christmas Came Early This Year",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Maecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "FREE TIME",
                    SectionColor = Color.FromRgba("#707525"),
                    Author =  "Jhon Deo",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a3.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_03.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Jhon Deo",
                    When =  "FEB 21, 2017",
                    Followers =  "120",
                },
                new ArticleData
                {
                    Id = 004,
                    Title =  "Morning Coffee Smells Sweet",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "SCIENCE",
                    SectionColor = Color.FromRgba("#2e66a1"),
                    Author =  "TLS SOFTWARE",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a4.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_04.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Janis Spector",
                    When =  "12/8/2018",
                    Followers =  "345",
                },
                new ArticleData
                {
                    Id = 005,
                    Title =  "Pleasant Colors in Garden",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "NATURE",
                    SectionColor = Color.FromRgba("#49ab81"),
                    Author =  "David Son",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a5.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_05.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerMeltdowns.mp44",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Julia Grant",
                    When =  "29 Dec, 2019",
                    Followers =  "2k",
                },
                new ArticleData
                {
                    Id = 006,
                    Title =  "My Shiny New Backpack",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "HEALTH",
                    SectionColor = Color.FromRgba("#e78b28"),
                    Author =  "Danielle Schneider",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a6.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_06.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Danielle Schneider",
                    When =  "JUN 12, 2017",
                    Followers =  "235",
                },
                new ArticleData
                {
                    Id = 007,
                    Title =  "Blooming Flowers in The House",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "FREE TIME",
                    SectionColor = Color.FromRgba("#ff647e"),
                    Author =  "Jhon Deo",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a7.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_07.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Jhon Deo",
                    When =  "FEB 21, 2017",
                    Followers =  "120",
                },
                new ArticleData
                {
                    Id = 008,
                    Title =  "Older Cars Never Out of Style",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "SCIENCE",
                    SectionColor = Color.FromRgba("#3680ab"),
                    Author =  "TLS SOFTWARE",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a8.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_08.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Janis Spector",
                    When =  "12/8/2018",
                    Followers =  "345",
                },
                new ArticleData
                {
                    Id = 009,
                    Title =  "Minimalist Interior Makeover",
                    Subtitle =  "Sed ut in perspiciatis unde omnis iste natus.",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc.\nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet.\n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                    Section = "NATURE",
                    SectionColor = Color.FromRgba("#49ab81"),
                    Author =  "David Son",
                    Avatar = AppSettings.ImageServerPath +  "avatars/a9.jpg",
                    BackgroundImage =  "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_09.jpg",
                    VideoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4",
                    Quote =  "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                    QuoteAuthor =  "Julia Grant",
                    When =  "29 Dec, 2019",
                    Followers =  "2k",
                },
            };
    }
}