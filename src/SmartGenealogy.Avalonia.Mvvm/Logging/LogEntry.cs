namespace SmartGenealogy.Avalonia.Mvvm.Logging;

public sealed class LogEntry
{
    public LogLevel LogLeel { get; set; } = LogLevel.Info;

    public string? Message { get; set; } = string.Empty;

    public SolidColorBrush Brush { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);
}