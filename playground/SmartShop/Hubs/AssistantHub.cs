using CoreAssistant;
using CoreAssistant.Models.Assistants;
using Microsoft.AspNetCore.SignalR;

namespace SmartShop.Hubs;

public class AssistantHub : Hub
{
    private readonly Assistant _assistant;

    public AssistantHub(Assistant assistant)
    {
        _assistant = assistant;
    }

    public async Task GenerateDescription(string question)
    {
        var img = await _assistant.Image.Generate(new ImagePrompt(question));
        if (img != null) { 
            await Clients.Caller.SendAsync("ImageResult", img.Url);
        }

        var answer = await _assistant.Chat.AskForSomething(new ChatQuestion(question));
        await Clients.Caller.SendAsync("DescriptionResult", answer.Content);

        var title = await _assistant.Chat.AskForSomething(new ChatQuestion("Give a title for this product"));
        await Clients.Caller.SendAsync("TitleResult", title.Content);

        await Clients.Caller.SendAsync("Done");
    }
}