namespace SmartGenealogy.Models;

public class Message : ObservableObject
{
    private string _content = string.Empty;

    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public Message(string content)
    {
        Content = content;
    }
}