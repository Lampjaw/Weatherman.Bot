using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weatherman.Discord
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddDiscordServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<DiscordOptions>(configuration);

            services.AddHostedService<DiscordListenerHostedService>();

            services.AddSingleton<IDiscordClientManager, DiscordClientManager>();
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            services.AddSingleton<ICommandManager, CommandManager>();
            services.AddSingleton<IWeatherLookupService, WeatherLookupService>();

            return services;
        }
    }
}
