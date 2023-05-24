using System.Text.Json.Serialization;

namespace  CoreAssistant.Models.Responses;

internal class ApiImageResponse
{
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("data")]
    public List<ImageData> Data { get; set; } = new();
}

internal class ImageData
{
    public string Url { get; set; } = string.Empty;
}