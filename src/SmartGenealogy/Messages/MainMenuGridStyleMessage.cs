namespace SmartGenealogy.Messages;

public class MainMenuGridStyleMessage : ValueChangedMessage<bool>
{
    public MainMenuGridStyleMessage(bool value) : base(value)
    {
    }
}