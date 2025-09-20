namespace SmartGenealogy.Messages;

public class LocalizedPickerBehaviorMessage : ValueChangedMessage<string>
{
    public LocalizedPickerBehaviorMessage(string lang) : base(lang)
    {
    }
}