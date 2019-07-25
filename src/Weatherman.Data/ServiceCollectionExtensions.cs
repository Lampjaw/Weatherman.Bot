using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weatherman.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
