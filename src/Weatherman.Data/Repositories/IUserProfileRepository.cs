using System.Threading.Tasks;

namespace Weatherman.Data.Repositories
{
    public interface IUserProfileRepository
    {
        Task<string> GetUsersDefaultLocationAsync(string userId);
        Task UpsertUserLastLocationAsync(string userId, object locationObject);
        Task UpsertUserHomeLocationAsync(string userId, object locationObject);
    }
}