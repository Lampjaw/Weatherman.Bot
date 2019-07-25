using System.Threading.Tasks;
using Weatherman.App.Models;

namespace Weatherman.App.Services
{
    public interface IUserManager
    {
        Task<GeoLocation> GetUsersDefaultLocationAsync(string userId);
        Task UpdateUserLastLocationAsync(string userId, GeoLocation location);
        Task UpdateUserHomeLocationAsync(string userId, GeoLocation location);
    }
}