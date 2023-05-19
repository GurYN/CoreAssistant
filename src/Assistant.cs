using CoreAssistant.Models;
using CoreAssistant.Repositories;
using CoreAssistant.Services;

namespace CoreAssistant;

public class Assistant
{
    private readonly Options _options;
    private readonly ApiService _apiService;
    private readonly HistoryRepository _historyRepository;

    public Assistant(Options options)
    {
        this._options = options;
        this._apiService = new ApiService(this._options.ApiKey);
        this._historyRepository = new HistoryRepository(this._options.DefaultContext);
    }

    public async Task<Answer> AskForSomething(Question question)
    {        
        var request = new ApiRequest(AddNewMessageAndGetHistory(question), _options.Model);
        var result = await _apiService.GetAnswer(request);

        this._historyRepository.AddHistoryItem(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.Assistant.ToString(),
                Content = result.Choices[0].Message.Content
            }
        });

        return result.ToAnswer();
    }

    public async IAsyncEnumerable<Answer> AskForSomethingAsStream(Question question)
    {
        var request = new ApiRequest(AddNewMessageAndGetHistory(question), _options.Model);

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
