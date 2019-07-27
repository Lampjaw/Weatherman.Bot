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
            using (var dbContext = factory.GetDbContext())
            {
                var userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(a => a.Id == userId);

                if (userProfile == null)
                {
                    return null;
                }

                return userProfile.HomeLocation ?? userProfile.LastLocation;
            }
        }

        public async Task UpsertUserLastLocationAsync(string userId, object locationObject)
        {
            var serializedLocation = JToken.FromObject(locationObject).ToString();
            var saveDate = DateTime.UtcNow;

            using (var factory = _dbContextHelper.GetFactory())
            using (var dbContext = factory.GetDbContext())
            {
                await dbContext.UserProfiles.Upsert(new UserProfile
                {
                    Id = userId,
                    LastLocation = serializedLocation,
                    LastLocationChangedDate = saveDate
                })
                .On(a => a.Id)
                .WhenMatched(a => new UserProfile
                {
                    LastLocation = serializedLocation,
                    LastLocationChangedDate = saveDate
                })
                .RunAsync();
            }
        }

        public async Task UpsertUserHomeLocationAsync(string userId, object locationObject)
        {
            var serializedLocation = JToken.FromObject(locationObject).ToString();

            using (var factory = _dbContextHelper.GetFactory())
            using (var dbContext = factory.GetDbContext())
            {
                await dbContext.UserProfiles.Upsert(new UserProfile
                {
                    Id = userId,
                    HomeLocation = serializedLocation,
                    HomeLocationChangedDate = DateTime.UtcNow
                })
                .On(a => a.Id)
                .WhenMatched(a => new UserProfile
                {
                    HomeLocation = serializedLocation,
                    HomeLocationChangedDate = DateTime.UtcNow
                })
                .RunAsync();
            }
        }
    }
}
