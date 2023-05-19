using System.Text.Json.Serialization;

namespace CoreAssistant.Models;

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