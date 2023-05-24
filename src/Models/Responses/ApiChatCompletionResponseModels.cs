using System.Text.Json.Serialization;
using CoreAssistant.Models;
using CoreAssistant.Models.Requests;

namespace CoreAssistant.Models.Responses;

internal class ApiChatCompletionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("created")]
    public int Created { get; set; }

    [JsonPropertyName("choices")]
    public List<ChoiceItem> Choices { get; set; } = new();

    [JsonPropertyName("usage")]
    public UsageItem Usage { get; set; } = new();

    internal ChatAnswer ToAnswer() {
        return new ChatAnswer() {
            Content = Choices[0].Message.Content,
            TokenUsed = Usage.TotalTokens
        };
    }
}

internal class ChoiceItem
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public ApiMessageItem Message { get; set; } = new();

    [JsonPropertyName("delta")]
    public ApiMessageItem Delta { set => Message = value; }
}

internal class UsageItem
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int AnswerTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}