using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Weatherman.Discord
{
    public class DiscordListenerHostedService : IHostedService
    {
        private readonly IDiscordClientManager _clientManager;
        private readonly IMessageProcessor _messageProcessor;

        public DiscordListenerHostedService(IDiscordClientManager clientManager, IMessageProcessor messageProcessor)
        {
            _clientManager = clientManager;
            _messageProcessor = messageProcessor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _clientManager.InitializeClientAsync();

            _clientManager.Client.MessageReceived += _messageProcessor.ProcessAsync;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _clientManager.DisconnectClientAsync();
            _clientManager.Dispose();
        }
    }
}
