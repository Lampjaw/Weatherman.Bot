using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weatherman.App.Clients.Here
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHereServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<HereOptions>(configuration);

            services.AddTransient<IHereClient, HereClient>();

            return services;
        }
    }
}
