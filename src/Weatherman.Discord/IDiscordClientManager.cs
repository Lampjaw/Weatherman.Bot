using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public interface IDiscordClientManager: IDisposable
    {
        DiscordSocketClient Client { get; }
        Task InitializeClientAsync();
        Task DisconnectClientAsync();
        SocketSelfUser GetClientUser();
    }
}