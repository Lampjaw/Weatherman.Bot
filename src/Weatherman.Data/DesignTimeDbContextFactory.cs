using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Weatherman.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeathermanDbContext>
    {
        public WeathermanDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("devsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<WeathermanDbContext>();

            var connectionString = configuration.GetValue<string>("DBConnectionString");

            builder.UseNpgsql(connectionString, o =>
            {
                o.CommandTimeout(7200);
            });

            return new WeathermanDbContext(builder.Options);
        }
    }
}
