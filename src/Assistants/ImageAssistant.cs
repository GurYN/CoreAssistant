using CoreAssistant.Models;
using CoreAssistant.Models.Requests;
using CoreAssistant.Services;

namespace CoreAssistant.Assistants;

public class ImageAssistant
{
    private readonly ApiService _apiService;

    internal ImageAssistant(ApiService api)
    {
        this._apiService = api;
    }

    public async Task<ImageResult?> Generate(ImagePrompt prompt) 
    {
        var request = new ApiImageRequest(prompt.Content);
        var result = await _apiService.GenerateImage(request);

        return result.Data.Count > 0 ? new ImageResult() { Url = result.Data[0].Url } : null;
    }
}