namespace SmartGenealogy.Messages;

public class DatabaseChangeMessage : ValueChangedMessage<string>
{
    public DatabaseChangeMessage(string value) : base(value)
    {
    }
}