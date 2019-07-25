using System.Threading.Tasks;

namespace Weatherman.Data.Repositories
{
    public interface IUserProfileRepository
    {
        Task<string> GetUsersDefaultLocationAsync(string userId);
        Task UpdateUserLastLocationAsync(string userId, object locationObject);
        Task UpdateUserHomeLocationAsync(string userId, object locationObject);
    }
}