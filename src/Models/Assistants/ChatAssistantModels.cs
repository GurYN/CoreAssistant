namespace CoreAssistant.Models;

public class ChatQuestion
{
    public string Content { get; set; } = string.Empty;

    public ChatQuestion(string content)
    {
        Content = content;
    }
}

public class ChatAnswer
{
    public string Content { get; set; } = string.Empty;
    public int TokenUsed { get; set; }
}

public class ChatModel 
{
    private string Value { get; set; }
    private ChatModel(string value) => Value = value;

    public static ChatModel GPT3_5 { get => new ChatModel("gpt-3.5-turbo"); }
    public static ChatModel GPT4 { get => new ChatModel("gpt-4"); }

    public override string ToString()
    {
        return Value;
    }
}