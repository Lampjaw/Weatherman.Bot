using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weatherman.App.Clients.DarkSky
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddDarkSkyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<DarkSkyOptions>(configuration);

            services.AddTransient<IDarkSkyClient, DarkSkyClient>();

            return services;
        }
    }
}
