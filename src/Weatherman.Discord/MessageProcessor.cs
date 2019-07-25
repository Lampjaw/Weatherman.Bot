using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public class MessageProcessor: IMessageProcessor
    {
        private readonly IDiscordClientManager _clientManager;
        private readonly ICommandManager _commandManager;

        public MessageProcessor(IDiscordClientManager clientManager, ICommandManager commandManager)
        {
            _clientManager = clientManager;
            _commandManager = commandManager;
        }

        public Task ProcessAsync(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User)
            {
                return Task.CompletedTask;
            }

            var context = new SocketCommandContext(_clientManager.Client, message);

            int argPos = 0;
            if (message.HasMentionPrefix(_clientManager.GetClientUser(), ref argPos))
            {
                return _commandManager.ExecuteAsync(context, argPos);
            }

            if (message.HasCharPrefix('!', ref argPos))
            {
                return _commandManager.ExecuteAsync(context, argPos);
            }

            return Task.CompletedTask;
        }
    }
}
