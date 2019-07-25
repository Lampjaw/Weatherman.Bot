using System.Threading.Tasks;

namespace Weatherman.Data.Repositories
{
    public interface IServerProfileRepository
    {
        Task UpdateServerPrefixAsync(string guildId, string userId, string prefix);
        Task<string> GetServerPrefixAsync(string guildId);
    }
}