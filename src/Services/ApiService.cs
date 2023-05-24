using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreAssistant.Models.Requests;
using CoreAssistant.Models.Responses;

namespace CoreAssistant.Services;

internal class ApiService
{
    private readonly HttpClient _api;

    internal ApiService(string apiKey)
    {
        this._api = new HttpClient();
        this._api.BaseAddress = new Uri("https://api.openai.com");
        this._api.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    internal async Task<ApiChatCompletionResponse> GetAnswer(ApiChatCompletionRequest questionContent)
    {
        var response = await this._api.PostAsJsonAsync("/v1/chat/completions", questionContent);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Error while getting answer from API, status code is {response.StatusCode.ToString()}");
        }

        var content = await response.Content.ReadFromJsonAsync<ApiChatCompletionResponse>();
        if (content is null) { throw new Exception("Error while getting content from API, content is empty."); }

        return content;
    }

    internal async IAsyncEnumerable<ApiChatCompletionResponse> GetAnswerAsStream(ApiChatCompletionRequest questionContent)
    {
        questionContent.Stream = true;

        var content = 
            JsonContent.Create(questionContent, null, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            });

        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        request.Content = content;

        var cancellationToken = new CancellationToken();
        using var response = _api.Send(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var data = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(data)) { continue; }

            data = data.StartsWith("data: ", StringComparison.Ordinal) ? data.Substring("data: ".Length) : data;

            if (data.StartsWith("[DONE]")) { break; }

            ApiChatCompletionResponse? chunk;
            try
            {
                chunk = JsonSerializer.Deserialize<ApiChatCompletionResponse>(data);
            }
            catch (Exception)
            {
                data += await reader.ReadToEndAsync();
                chunk = JsonSerializer.Deserialize<ApiChatCompletionResponse>(data);
            }

            if (chunk != null) { yield return chunk; }
        }
    }

    internal async Task<ApiImageResponse> GenerateImage(ApiImageRequest imageRequest)
    {
        var response = await this._api.PostAsJsonAsync("/v1/images/generations", imageRequest);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Error while getting image from API, status code is {response.StatusCode.ToString()}");
        }

        var content = await response.Content.ReadFromJsonAsync<ApiImageResponse>();
        if (content is null) { throw new Exception("Error while getting content from API, content is empty."); }

        return content;
    }
}