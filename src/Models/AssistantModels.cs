namespace CoreAssistant;

public class Options
{
    public string ApiKey { get; private set; }
    public AssistantModel Model { get; set; } = AssistantModel.GPT3_5;
    public string DefaultContext { get; set; } = "You're a smart assistant able to answer to any question.";
    public int MaxToken { get; set; } = 4096;
    public string User { get; set; } = "CoreAssistant";

    public Options(string apiKey)
    {
        this.ApiKey = apiKey;
    }
}

public class Question
{
    public string Content { get; set; } = string.Empty;

    public Question(string content)
    {
        Content = content;
    }
}

public class Answer
{
    public string Content { get; set; } = string.Empty;
    public int TokenUsed { get; set; }
}

public class AssistantModel 
{
    private string Value { get; set; }
    private AssistantModel(string value) => Value = value;

    public static AssistantModel GPT3_5 { get => new AssistantModel("gpt-3.5-turbo"); }
    public static AssistantModel GPT4 { get => new AssistantModel("gpt-4"); }

    public override string ToString()
    {
        return Value;
    }
}