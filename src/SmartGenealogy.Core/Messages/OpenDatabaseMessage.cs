namespace SmartGenealogy.Core.Messages;

/// <summary>
/// Open database message.
/// </summary>
public class OpenDatabaseMessage : ValueChangedMessage<bool>
{
    public OpenDatabaseMessage(bool value)
        : base(value)
    {
    }
}