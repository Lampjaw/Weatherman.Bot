using System.Threading.Tasks;
using Weatherman.Data.Repositories;

namespace Weatherman.App.Services
{
    public class ServerManager : IServerManager
    {
        private readonly IServerProfileRepository _repository;

        public ServerManager(IServerProfileRepository repository)
        {
            _repository = repository;
        }

        public Task<string> GetServerPrefixAsync(string guildId)
        {
            return _repository.GetServerPrefixAsync(guildId);
        }

        public Task UpdateServerPrefixAsync(string guildId, string userId, string prefix)
        {
            return _repository.UpdateServerPrefixAsync(guildId, userId, prefix);
        }
    }
}
