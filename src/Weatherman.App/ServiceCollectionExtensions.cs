using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weatherman.App.Clients.DarkSky;
using Weatherman.App.Clients.Here;
using Weatherman.App.Clients.YahooWeather;
using Weatherman.App.Services;

namespace Weatherman.App
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHereServices(configuration);
            services.AddYahooWeatherServices(configuration);
            services.AddDarkSkyServices(configuration);

            services.AddSingleton<IWeatherService, WeatherService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<IWeatherSearchService, WeatherSearchService>();

            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IServerManager, ServerManager>();

            return services;
        }
    }
}