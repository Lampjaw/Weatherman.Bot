using Discord.WebSocket;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(SocketMessage message);
    }
}