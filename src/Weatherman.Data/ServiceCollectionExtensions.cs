using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Weatherman.Data.Repositories;

namespace Weatherman.Data
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string _migrationAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly.GetName().Name;

        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<DataOptions>(configuration);

            var options = configuration.Get<DataOptions>();

            services.AddEntityFrameworkNpgsql();

            services.AddDbContextPool<WeathermanDbContext>(builder =>
                builder.UseNpgsql(options.DBConnectionString, b => {
                    b.MigrationsAssembly(_migrationAssembly);
                }), options.PoolSize);

            services.AddSingleton<IDbContextHelper, DbContextHelper>();
            services.AddSingleton<IUserProfileRepository, UserProfileRepository>();
            services.AddSingleton<IServerProfileRepository, ServerProfileRepository>();

            return services;
        }
    }
}
