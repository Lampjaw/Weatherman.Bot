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
            {
                var dbContext = factory.GetDbContext();

                var result = await dbContext.ServerProfiles.FirstOrDefaultAsync(a => a.Id == guildId);

                return result?.Prefix;
            }
        }

        public async Task UpdateServerPrefixAsync(string guildId, string userId, string prefix)
        {
            using (var factory = _dbContextHelper.GetFactory())
            {
                var dbContext = factory.GetDbContext();

                var serverProfile = await dbContext.ServerProfiles.FirstOrDefaultAsync(a => a.Id == guildId);

                if (serverProfile == null)
                {
                    serverProfile = new ServerProfile
                    {
                        Id = guildId,
                        LastChangedBy = userId,
                        LastChangedDate = DateTime.UtcNow
                    };

                    dbContext.Add(serverProfile);
                    await dbContext.SaveChangesAsync();
                    return;
                }

                serverProfile.Prefix = prefix;
                serverProfile.LastChangedBy = userId;
                serverProfile.LastChangedDate = DateTime.UtcNow;

                dbContext.Update(serverProfile);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
