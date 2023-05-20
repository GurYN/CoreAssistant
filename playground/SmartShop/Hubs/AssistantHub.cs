using CoreAssistant;
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
        var answer = await _assistant.AskForSomething(new Question(question));
        await Clients.Caller.SendAsync("DescriptionResult", answer.Content);
    }
}