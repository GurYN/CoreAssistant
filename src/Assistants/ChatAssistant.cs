using CoreAssistant.Models;
using CoreAssistant.Models.Assistants;
using CoreAssistant.Models.Requests;
using CoreAssistant.Repositories;
using CoreAssistant.Services;

namespace CoreAssistant.Assistants;

public class ChatAssistant
{
    private readonly ApiService _apiService;
    private readonly HistoryRepository _historyRepository;

    internal ChatAssistant(ApiService api, string defaultContext)
    {
        this._apiService = api;
        this._historyRepository = new HistoryRepository(defaultContext);
    }

        public async Task<ChatAnswer> AskForSomething(ChatQuestion question, ChatModel? model = null)
    {        
        var request = new ApiChatCompletionRequest(AddNewMessageAndGetHistory(question), model != null ? model : ChatModel.GPT3_5);
        var result = await _apiService.GetAnswer(request);

        this._historyRepository.AddHistoryItem(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.Assistant.ToString(),
                Content = result.Choices[0].Message.Content
            }
        });

        return result.ToAnswer();
    }

    public async IAsyncEnumerable<ChatAnswer> AskForSomethingAsStream(ChatQuestion question, ChatModel? model = null)
    {
        var request = new ApiChatCompletionRequest(AddNewMessageAndGetHistory(question), model != null ? model : ChatModel.GPT3_5);
        var result = _apiService.GetAnswerAsStream(request);

        var content = "";
        await foreach (var item in result)
        {
            if (item.Choices.Count == 0) { continue; }

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

    public void ClearConversationHistory()
    {
        this._historyRepository.ClearHistory();
    }

    private List<ApiMessageItem> AddNewMessageAndGetHistory(ChatQuestion question)
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