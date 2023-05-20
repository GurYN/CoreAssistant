using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreAssistant.Models;

namespace CoreAssistant.Services;

internal class ApiService
{
    private readonly HttpClient _api;

    public ApiService(string apiKey)
    {
        this._api = new HttpClient();
        this._api.BaseAddress = new Uri("https://api.openai.com");
        this._api.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<ApiResponse> GetAnswer(ApiRequest questionContent)
    {
        var response = await this._api.PostAsJsonAsync("/v1/chat/completions", questionContent);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Error while getting answer from API, status code is {response.StatusCode.ToString()}");
        }

        var content = await response.Content.ReadFromJsonAsync<ApiResponse>();
        if (content is null) { throw new Exception("Error while getting content from API, content is empty."); }

        return content;
    }

    public async IAsyncEnumerable<ApiResponse> GetAnswerAsStream(ApiRequest questionContent)
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

            ApiResponse? chunk;
            try
            {
                chunk = JsonSerializer.Deserialize<ApiResponse>(data);
            }
            catch (Exception)
            {
                data += await reader.ReadToEndAsync();
                chunk = JsonSerializer.Deserialize<ApiResponse>(data);
            }

            if (chunk != null) { yield return chunk; }
        }
    }
}