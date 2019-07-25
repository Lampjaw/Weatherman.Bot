using System.Threading.Tasks;

namespace Weatherman.App.Services
{
    public interface IServerManager
    {
        Task UpdateServerPrefixAsync(string guildId, string userId, string prefix);
        Task<string> GetServerPrefixAsync(string guildId);
    }
}