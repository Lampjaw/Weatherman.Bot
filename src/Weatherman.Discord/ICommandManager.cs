using Discord.Commands;
using System.Threading.Tasks;

namespace Weatherman.Discord
{
    public interface ICommandManager
    {
        Task<IResult> ExecuteAsync(ICommandContext context, int argPos);
    }
}