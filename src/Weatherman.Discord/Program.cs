using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Weatherman.App;
using Weatherman.Data;

namespace Weatherman.Discord
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    var configuration = hostBuilderContext.Configuration;

                    services.AddDiscordServices(configuration);
                    services.AddApplicationServices(configuration);
                    services.AddDataServices(configuration);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddFilter((category, logLevel) =>
                    {
                        if (category.StartsWith("Microsoft.EntityFramework") && logLevel <= LogLevel.Warning)
                        {
                            return false;
                        }

                        return true;
                    });
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }
}
