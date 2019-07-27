using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Weatherman.Domain.Models;
using Weatherman.Data.Repositories;

namespace Weatherman.App.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserProfileRepository _repository;

        public UserManager(IUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<GeoLocation> GetUsersDefaultLocationAsync(string userId)
        {
            var result = await _repository.GetUsersDefaultLocationAsync(userId);
            if (result == null)
            {
                return null;
            }

            return JToken.Parse(result).ToObject<GeoLocation>();
        }

        public Task UpdateUserHomeLocationAsync(string userId, GeoLocation location)
        {
            return _repository.UpsertUserHomeLocationAsync(userId, location);
        }

        public Task UpdateUserLastLocationAsync(string userId, GeoLocation location)
        {
            return _repository.UpsertUserLastLocationAsync(userId, location);
        }
    }
}
