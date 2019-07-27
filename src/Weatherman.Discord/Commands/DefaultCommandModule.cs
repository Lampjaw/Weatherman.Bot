using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Weatherman.App.Services;
using Weatherman.Domain;
using Weatherman.Domain.Models;
using static Weatherman.Domain.Models.ForecastWeather;

namespace Weatherman.Discord.Commands
{
    public class DefaultCommandModule : ModuleBase<SocketCommandContext>
    {
        private const WeatherClient DEFAULT_WEATHER_CLIENT = WeatherClient.DarkSky;

        private readonly IWeatherLookupService _weatherLookupService;
        private readonly IServerManager _serverManager;
        private readonly IUserManager _userManager;
        private readonly ILocationService _locationService;

        public DefaultCommandModule(IWeatherLookupService weatherLookupService, IServerManager serverManager, IUserManager userManager, ILocationService locationService)
        {
            _weatherLookupService = weatherLookupService;
            _serverManager = serverManager;
            _userManager = userManager;
            _locationService = locationService;
        }

        [Command("w")]
        [Summary("Get the current weather for a location")]
        public async Task GetCurrentWeatherForecastAsync([Remainder] string locationText = null)
        {
            var userId = Context.User.Id.ToString();

            var result = await _weatherLookupService.GetCurrentWeatherByLocationTextAsync(userId, locationText, DEFAULT_WEATHER_CLIENT);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync("Something went wrong");
                return;
            }

            var description = string.Format("Currently {0} and {1} with a high of {2} and a low of {3}",
                ConvertToTempString(result.Temperature), result.Condition, ConvertToTempString(result.ForecastHigh), ConvertToTempString(result.ForecastLow));

            var embed = new EmbedBuilder()
                .WithAuthor(BuildLocationString(result.Location))
                .WithColor(0x070707)
                .WithDescription(description)
                .AddField("Wind Speed", $"{result.WindSpeed:F1} MpH", true)
                .AddField("Wind Chill", ConvertToTempString(result.WindChill), true)
                .AddField("Humidity", $"{result.Humidity * 100:F0}%", true)
                .AddField("Heat Index", ConvertToTempString(result.HeatIndex), true)
                .Build();

            await Context.Channel.SendMessageAsync(embed: embed);
        }

        [Command("wf")]
        [Summary("Get the weather forecast for a location")]
        public async Task GetFutureWeatherForecastAsync([Remainder] string locationText = null)
        {
            var userId = Context.User.Id.ToString();

            var result = await _weatherLookupService.GetFutureWeatherByLocationTextAsync(userId, locationText, DEFAULT_WEATHER_CLIENT);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync("Something went wrong");
                return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(BuildLocationString(result.Location))
                .WithColor(0x070707);

            foreach(var day in result.Forecast.Take(5))
            {
                embedBuilder.AddField(day.Date, CreateWeatherDay(day), false);
            }

            var embed = embedBuilder.Build();

            await Context.Channel.SendMessageAsync(embed: embed);
        }

        [Command("setprefix")]
        [Summary("Set the command prefix for the server")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public async Task SetServerPrefix(string prefix)
        {
            var userId = Context.User.Id.ToString();
            var guildId = Context.Guild.Id.ToString();

            await _serverManager.UpdateServerPrefixAsync(guildId, userId, prefix);

            await Context.Channel.SendMessageAsync("Prefix set!");
        }

        [Command("sethome")]
        [Summary("Set a home location")]
        public async Task SetHomeLocation([Remainder] string location)
        {
            var userId = Context.User.Id.ToString();

            var result = await _locationService.FindLocationByTextAsync(location);
            if (result == null)
            {
                await Context.Channel.SendMessageAsync("Something went wrong");
                return;
            }

            await _userManager.UpdateUserHomeLocationAsync(userId, result);

            await Context.Channel.SendMessageAsync("Home set!");
        }

        private string CreateWeatherDay(WeatherDay day)
        {
            var tempHigh = ConvertToTempString(day.High);
            var tempLow = ConvertToTempString(day.Low);
            return string.Format("{0}: {1} / {2} - {3}", day.Day, tempHigh, tempLow, day.Text);
        }

        private string ConvertToTempString(double temp)
        {
            var tempCelsius = ConvertToCelsius(temp);
            return string.Format("{0:F0} °F ({1:F0} °C)", temp, tempCelsius);
        }

        private double ConvertToCelsius(double temp)
        {
            return (temp - 32) / 1.8; ;
        }

        private string BuildLocationString(GeoLocation location)
        {
            var cityPart = string.IsNullOrWhiteSpace(location.City) ? "" : $"{location.City}, ";
            var regionPart = string.IsNullOrWhiteSpace(location.Region) ? "" : $"{location.Region} - ";

            return $"{cityPart}{regionPart}{location.Country}";
        }
    }
}
