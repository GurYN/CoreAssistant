using CoreAssistant.Models;
using CoreAssistant.Repositories;
using CoreAssistant.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreAssistant;

public class Assistant
{
    private readonly CoreAssistantOptions _options;
    private readonly ApiService _apiService;
    private readonly HistoryRepository _historyRepository;

    [ActivatorUtilitiesConstructor]
    public Assistant(IOptions<CoreAssistantOptions> options)
        : this(options.Value)
    {
    }

    public Assistant(CoreAssistantOptions options)
    {
        this._options = options;
        this._apiService = new ApiService(this._options.ApiKey);
        this._historyRepository = new HistoryRepository(this._options.DefaultContext);
    }

    public async Task<Answer> AskForSomething(Question question, AssistantModel? model = null)
    {        
        var request = new ApiRequest(AddNewMessageAndGetHistory(question), model != null ? model : AssistantModel.GPT3_5);
        var result = await _apiService.GetAnswer(request);

        this._historyRepository.AddHistoryItem(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.Assistant.ToString(),
                Content = result.Choices[0].Message.Content
            }
        });

        return result.ToAnswer();
    }

    public async IAsyncEnumerable<Answer> AskForSomethingAsStream(Question question, AssistantModel? model = null)
    {
        var request = new ApiRequest(AddNewMessageAndGetHistory(question), model != null ? model : AssistantModel.GPT3_5);
        var result = _apiService.GetAnswerAsStream(request);

        var content = "";
        await foreach (var item in result)
        {
            content += item.Choices[0].Message.Content;
            yield return item.ToAnswer();
        }

        this._historyRepository.AddHistoryItem(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.Assistant.ToString(),
                Content = content
            }
        });
    }

    public void ClearHistory()
    {
        this._historyRepository.ClearHistory();
    }

    private List<ApiMessageItem> AddNewMessageAndGetHistory(Question question)
    {
        this._historyRepository.AddHistoryItem(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.User.ToString(),
                Content = question.Content
            }
        });

        return this._historyRepository.GetHistory().Select(x => x.Message).ToList();
    }
}
