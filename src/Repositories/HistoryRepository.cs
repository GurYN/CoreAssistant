using CoreAssistant.Models;
using CoreAssistant.Models.Requests;

namespace CoreAssistant.Repositories;

internal class HistoryRepository
{
    private readonly List<HistoryItem> _historyItems = new();
    
    internal HistoryRepository(string context)
    {
        this._historyItems.Add(new HistoryItem() {
            Message = new ApiMessageItem() {
                Role = MessageRole.System.ToString(),
                Content = context
            }
        });
    }

    internal List<HistoryItem> AddHistoryItem(HistoryItem item)
    {
        this._historyItems.Add(item);
        return this._historyItems;
    }

    internal List<HistoryItem> ClearHistory()
    {
        this._historyItems.RemoveRange(1, this._historyItems.Count - 1);
        return this._historyItems;
    }

    internal List<HistoryItem> GetHistory()
    {
        return _historyItems;
    }
}