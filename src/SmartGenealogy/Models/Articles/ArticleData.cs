namespace SmartGenealogy.Models.Articles;

public class ArticleData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Body { get; set; }
    public string Section { get; set; }
    public Color SectionColor { get; set; }
    public string Author { get; set; }
    public string Avatar { get; set; }
    public string BackgroundImage { get; set; }
    public string VideoUrl { get; set; }
    public string Quote { get; set; }
    public string QuoteAuthor { get; set; }
    public string When { get; set; }
    public string Followers { get; set; }
}