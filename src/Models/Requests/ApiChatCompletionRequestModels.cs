using System.Text.Json.Serialization;
using CoreAssistant.Models.Assistants;

namespace CoreAssistant.Models.Requests;

internal class ApiChatCompletionRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = ChatModel.GPT3_5.ToString();

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("messages")]    
    public List<ApiMessageItem> Messages { get; set; } = new();

    internal ApiChatCompletionRequest(List<ApiMessageItem> messages, ChatModel? model = null)
    {
        if (model != null) {
            this.Model = model.ToString();
        }

        this.Messages.AddRange(messages);
    }
}

internal class ApiMessageItem
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = MessageRole.User.ToString();

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

internal class MessageRole 
{
    private string Value { get; set; }
    private MessageRole(string value) => Value = value;

    public static MessageRole System { get => new MessageRole("system"); }
    public static MessageRole User { get => new MessageRole("user"); }
    public static MessageRole Assistant { get => new MessageRole("assistant"); }

    public override string ToString()
    {
        return Value;
    }
}