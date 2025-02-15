using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SmartGenealogy.Messages;

public class FileOpenedMessage : ValueChangedMessage<string>
{
    public FileOpenedMessage(string filePath)
        : base(filePath)
    {
    }
}