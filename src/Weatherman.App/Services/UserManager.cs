using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Weatherman.App.Models;
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
            return _repository.UpdateUserHomeLocationAsync(userId, location);
        }

        public Task UpdateUserLastLocationAsync(string userId, GeoLocation location)
        {
            return _repository.UpdateUserLastLocationAsync(userId, location);
        }
    }
}
