using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public class DiscordClientManager: IDiscordClientManager
    {
        private readonly DiscordOptions _options;
        private readonly ILogger<DiscordClientManager> _logger;

        public DiscordSocketClient Client { get; private set; }

        public DiscordClientManager(IOptions<DiscordOptions> options, ILogger<DiscordClientManager> logger)
        {
            _options = options.Value;
            _logger = logger;

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
        }

        public async Task InitializeClientAsync()
        {
            if (Client != null && Client.ConnectionState == ConnectionState.Connected)
            {
                return;
            }

            await Client.LoginAsync(TokenType.Bot, _options.DiscordToken);
            await Client.StartAsync();

            Client.Log += HandleLog;
        }

        public Task DisconnectClientAsync()
        {
            return Client?.StopAsync();
        }

        private Task HandleLog(LogMessage log)
        {
            _logger.LogInformation(log.ToString());
            return Task.CompletedTask;
        }

        public SocketSelfUser GetClientUser()
        {
            return Client?.CurrentUser;
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
