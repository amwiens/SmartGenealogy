namespace SmartGenealogy.Controls.Videos;

public class ResourceVideoSource : VideoSource
{
    public static readonly BindableProperty PathProperty =
        BindableProperty.Create(nameof(Path), typeof(string), typeof(ResourceVideoSource));

    public string Path
    {
        get => (string)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }
}