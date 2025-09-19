namespace SmartGenealogy.Messages;

public class DatabaseOpenMessage : ValueChangedMessage<string>
{
    public DatabaseOpenMessage(string value)
        : base(value)
    { }
}