using Microsoft.Extensions.DependencyInjection;

namespace CoreAssistant.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreAssistant(this IServiceCollection services, Action<CoreAssistantOptions> options)
    {
        var optionsBuilder = services.AddOptions<CoreAssistantOptions>();
        optionsBuilder.Configure(options);

        return services.AddScoped<Assistant>();
    }
}