using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Weatherman.App.Services;

namespace Weatherman.Discord
{
    public class MessageProcessor: IMessageProcessor
    {
        private readonly IDiscordClientManager _clientManager;
        private readonly ICommandManager _commandManager;
        private readonly IServerManager _serverManager;
        private readonly string _defaultPrefix;

        public MessageProcessor(IOptions<DiscordOptions> options, IDiscordClientManager clientManager, ICommandManager commandManager, IServerManager serverManager)
        {
            _clientManager = clientManager;
            _commandManager = commandManager;
            _serverManager = serverManager;

            _defaultPrefix = options.Value.DefaultPrefix ?? "?";
        }

        public async Task ProcessAsync(SocketMessage rawMessage)
        {
            var prefix = _defaultPrefix;

            if (rawMessage.Channel is SocketTextChannel channel)
            {
                var guildId = channel.Guild.Id.ToString();

                var storedPrefix = await _serverManager.GetServerPrefixAsync(guildId);
                prefix = storedPrefix ?? prefix;
            }

            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User)
            {
                return;
            }

            var context = new SocketCommandContext(_clientManager.Client, message);

            int argPos = 0;
            if (message.HasMentionPrefix(_clientManager.GetClientUser(), ref argPos))
            {
                await _commandManager.ExecuteAsync(context, argPos);
                return;
            }

            if (message.HasStringPrefix(prefix, ref argPos))
            {
                await _commandManager.ExecuteAsync(context, argPos);
                return;
            }
        }
    }
}
