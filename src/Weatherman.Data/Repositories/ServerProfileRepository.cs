using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Weatherman.Data.Models;

namespace Weatherman.Data.Repositories
{
    public class ServerProfileRepository : IServerProfileRepository
    {
        private readonly IDbContextHelper _dbContextHelper;

        public ServerProfileRepository(IDbContextHelper dbContextHelper)
        {
            _dbContextHelper = dbContextHelper;
        }

        public async Task<string> GetServerPrefixAsync(string guildId)
        {
            using (var factory = _dbContextHelper.GetFactory())
            using (var dbContext = factory.GetDbContext())
            {
                var result = await dbContext.ServerProfiles.FirstOrDefaultAsync(a => a.Id == guildId);

                return result?.Prefix;
            }
        }

        public async Task UpsertServerPrefixAsync(string guildId, string userId, string prefix)
        {
            using (var factory = _dbContextHelper.GetFactory())
            using (var dbContext = factory.GetDbContext())
            {
                await dbContext.ServerProfiles.Upsert(new ServerProfile
                {
                    Id = guildId,
                    Prefix = prefix,
                    LastChangedBy = userId,
                    LastChangedDate = DateTime.UtcNow
                })
                .On(a => a.Id)
                .WhenMatched(a => new ServerProfile
                {
                    Prefix = prefix,
                    LastChangedBy = userId,
                    LastChangedDate = DateTime.UtcNow
                })
                .RunAsync();
            }
        }
    }
}
