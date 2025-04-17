using Microsoft.Extensions.AI;

namespace SmartGenealogy.ViewModels.Settings;

public partial class AISettingsViewModel : BaseViewModel
{
    private readonly IChatClient _chatClient;

    public AISettingsViewModel(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task OnNavigatedToAsync()
    {
        var message = new ChatMessage(ChatRole.User, "What's in this image?"); // "C:\Users\amwie\OneDrive\Genealogy\RootsMagic8\Media\People\Aafedt\Aafedt, Joan Grace (1960)\Pics\AafedtJoanGrace-1978-Yearbook_profile_photo.jpg"
        message.Contents.Add(new DataContent(File.ReadAllBytes("C:\\Users\\amwie\\OneDrive\\Genealogy\\RootsMagic8\\Media\\People\\Aldrich\\Aldrich, Clifford Sidney (1895)\\Pics\\gravestone2.jpg"), "image/jpeg"));

        var response = await _chatClient.GetResponseAsync(message);

        await Task.CompletedTask;
    }

    public async Task OnNavigatedFromAsync()
    {
        await Task.CompletedTask;
    }
}