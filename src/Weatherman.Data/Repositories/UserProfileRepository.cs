using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Weatherman.Data.Models;

namespace Weatherman.Data.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IDbContextHelper _dbContextHelper;

        public UserProfileRepository(IDbContextHelper dbContextHelper)
        {
            _dbContextHelper = dbContextHelper;
        }

        public async Task<string> GetUsersDefaultLocationAsync(string userId)
        {
            using (var factory = _dbContextHelper.GetFactory())
            {
                var dbContext = factory.GetDbContext();

                var userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(a => a.Id == userId);

                if (userProfile == null)
                {
                    return null;
                }

                return userProfile.HomeLocation ?? userProfile.LastLocation;
            }
        }

        public async Task UpdateUserLastLocationAsync(string userId, object locationObject)
        {
            using (var factory = _dbContextHelper.GetFactory())
            {
                var dbContext = factory.GetDbContext();

                var userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(a => a.Id == userId);

                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        Id = userId,
                        LastLocation = JToken.FromObject(locationObject).ToString(),
                        LastLocationChangedDate = DateTime.UtcNow
                    };

                    dbContext.Add(userProfile);
                    await dbContext.SaveChangesAsync();
                    return;
                }

                userProfile.LastLocation = JToken.FromObject(locationObject).ToString();
                userProfile.LastLocationChangedDate = DateTime.UtcNow;

                dbContext.Update(userProfile);

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUserHomeLocationAsync(string userId, object locationObject)
        {
            using (var factory = _dbContextHelper.GetFactory())
            {
                var dbContext = factory.GetDbContext();

                var userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(a => a.Id == userId);

                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        Id = userId,
                        HomeLocation = JToken.FromObject(locationObject).ToString(),
                        HomeLocationChangedDate = DateTime.UtcNow
                    };

                    dbContext.Add(userProfile);
                    await dbContext.SaveChangesAsync();
                    return;
                }

                userProfile.HomeLocation = JToken.FromObject(locationObject).ToString();
                userProfile.HomeLocationChangedDate = DateTime.UtcNow;

                dbContext.Update(userProfile);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
