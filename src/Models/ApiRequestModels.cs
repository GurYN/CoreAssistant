using System.Text.Json.Serialization;

namespace CoreAssistant.Models;

internal class ApiRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = AssistantModel.GPT3_5.ToString();

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("messages")]    
    public List<ApiMessageItem> Messages { get; set; } = new();

    public ApiRequest(List<ApiMessageItem> messages, AssistantModel? model = null)
    {
        if (model != null) {
            this.Model = model.ToString();
        }

        this.Messages.AddRange(messages);
    }
}