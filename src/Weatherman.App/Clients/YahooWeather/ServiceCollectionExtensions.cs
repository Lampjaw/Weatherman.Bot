using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weatherman.App.Clients.YahooWeather
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddYahooWeatherServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<YahooWeatherOptions>(configuration);

            services.AddTransient<IYahooWeatherClient, YahooWeatherClient>();

            return services;
        }
    }
}
