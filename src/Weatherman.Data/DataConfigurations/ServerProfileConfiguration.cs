using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weatherman.Data.Models;

namespace Weatherman.Data.DataConfigurations
{
    internal class ServerProfileConfiguration : IEntityTypeConfiguration<ServerProfile>
    {
        public void Configure(EntityTypeBuilder<ServerProfile> builder)
        {
            builder.ToTable("ServerProfile");

            builder.HasKey(a => a.Id);
        }
    }
}
