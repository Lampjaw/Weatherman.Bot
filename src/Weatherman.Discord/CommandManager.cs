using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public class CommandManager: ICommandManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandManager> _logger;
        private readonly CommandService _service;

        public CommandManager(IServiceProvider serviceProvider, ILogger<CommandManager> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _service = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _service.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            _service.Log += HandleLog;
            _service.CommandExecuted += CommandExecutedAsync;
        }

        public Task<IResult> ExecuteAsync(ICommandContext context, int argPos)
        {
            return _service.ExecuteAsync(context, argPos, _serviceProvider);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result.ErrorReason}");
        }

        private Task HandleLog(LogMessage log)
        {
            _logger.LogInformation(log.ToString());
            return Task.CompletedTask;
        }
    }
}
