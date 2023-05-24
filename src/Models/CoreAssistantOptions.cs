namespace CoreAssistant;

public class CoreAssistantOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string DefaultContext { get; set; } = "You're a smart assistant able to answer to any question.";

    public CoreAssistantOptions() { }

    public CoreAssistantOptions(string apiKey)
    {
        this.ApiKey = apiKey;
    }
}