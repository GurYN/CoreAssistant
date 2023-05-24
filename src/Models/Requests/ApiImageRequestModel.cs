using System.Text.Json.Serialization;
using CoreAssistant.Models;

namespace CoreAssistant.Models.Requests;

internal class ApiImageRequest 
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; private set; }
    [JsonPropertyName("n")]
    public int? N { get; set; } = 1;
    [JsonPropertyName("size")]
    public string? Size { get; set; } = ImageSize.Resolution_1024x1024.ToString();

    internal ApiImageRequest(string prompt)
    {
        this.Prompt = prompt;
    }
}