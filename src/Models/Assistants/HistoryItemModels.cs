using CoreAssistant.Models.Requests;

namespace CoreAssistant.Models;

internal class HistoryItem
{
    public DateTime Created { get; set; } = DateTime.Now;
    public ApiMessageItem Message { get; set; } = new();
}