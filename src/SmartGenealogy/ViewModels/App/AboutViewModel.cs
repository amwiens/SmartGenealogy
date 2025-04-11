namespace SmartGenealogy.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    public ObservableCollection<TestimonialModel> items;

    public AboutViewModel()
    {
        HeaderImage = AppSettings.ImageServerPath + "15.jpg";
        LoadTestimonials();
    }

    private void LoadTestimonials()
    {
        items = new ObservableCollection<TestimonialModel>
        {
            new TestimonialModel
            {
                Avatar = AppSettings.ImageServerPath +  "avatars/user1.png",
                UserName = "Alice Russell",
                Professional = "Senior Manager",
                Rating = 5.0,
                Comment = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                ImageUrl = AppSettings.ImageServerPath +  "brands/microsoft_logo.png"
            },
            new TestimonialModel
            {
                Avatar = AppSettings.ImageServerPath +  "avatars/user2.png",
                UserName = "David Son",
                Professional = "Director",
                Rating = 4.5,
                Comment = "Spread all the pieces of paper onto a table, a desk, a bed, or even the floor.",
                ImageUrl = AppSettings.ImageServerPath +  "brands/xamarin-forms-logo.png"
            },
            new TestimonialModel
            {
                Avatar = AppSettings.ImageServerPath +  "avatars/user3.png",
                UserName = "Cecily Trujillo",
                Professional = "Founder & CTO",
                Rating = 5.0,
                Comment = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                ImageUrl = AppSettings.ImageServerPath +  "brands/microsoft_logo.png"
            },
            new TestimonialModel
            {
                Avatar = AppSettings.ImageServerPath +  "avatars/user4.png",
                UserName = "Antoni Whitney",
                Professional = "Found & CEO",
                Rating = 4.5,
                Comment = "Spread all the pieces of paper onto a table, a desk, a bed, or even the floor.",
                ImageUrl = AppSettings.ImageServerPath +  "brands/xamarin-forms-logo.png"
            }
        };

        TestimonialItems = new ObservableCollection<TestimonialModel>(items);
    }

    [ObservableProperty]
    private string headerImage;

    [ObservableProperty]
    private ObservableCollection<TestimonialModel> _testimonialItems;

    [RelayCommand]
    private void Tap()
    {

    }
}

public class TestimonialModel
{
    public string Avatar { get; set; }
    public string UserName { get; set; }
    public string Professional { get; set; }
    public double Rating { get; set; }
    public string Comment { get; set; }
    public string ImageUrl { get; set; }
}