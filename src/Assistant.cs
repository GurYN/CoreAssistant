using CoreAssistant.Assistants;
using CoreAssistant.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreAssistant;

public class Assistant
{
    private readonly CoreAssistantOptions _options;
    private readonly ApiService _apiService;

    private Lazy<ChatAssistant> _chat;
    public ChatAssistant Chat { get { return _chat.Value; } }

    private Lazy<ImageAssistant> _image;
    public ImageAssistant Image { get { return _image.Value; } }

    [ActivatorUtilitiesConstructor]
    public Assistant(IOptions<CoreAssistantOptions> options)
        : this(options.Value)
    {
    }

    public Assistant(CoreAssistantOptions options)
    {
        this._options = options;
        this._apiService = new ApiService(this._options.ApiKey);

        this._chat = new Lazy<ChatAssistant>(() => new ChatAssistant(_apiService, _options.DefaultContext));
        this._image = new Lazy<ImageAssistant>(() => new ImageAssistant(_apiService));
    }
}
