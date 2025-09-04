namespace SmartGenealogy.Messages;

public class DatabaseChangeMessage : ValueChangedMessage<bool>
{
    public DatabaseChangeMessage(bool value) : base(value)
    {
    }
}