namespace SmartGenealogy.Core.Models.Settings;

public class Settings
{
    public int? Version { get; set; } = 1;

    public WindowSettings? WindowSettings { get; set; }

    public float AnimationScale { get; set; } = 1.0f;

    public Dictionary<NotificationKey, NotificationOption> NotificationOptions { get; set; } = new();
}