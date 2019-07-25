using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Weatherman.App.Services;
using static Weatherman.App.Models.ForecastWeather;

namespace Weatherman.Discord.Commands
{
    public class DefaultCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly IWeatherSearchService _weatherSearchService;

        public DefaultCommandModule(IWeatherSearchService weatherSearchService)
        {
            _weatherSearchService = weatherSearchService;
        }

        [Command("w")]
        [Summary("Get the current weather for a location")]
        public async Task GetCurrentWeatherForecastAsync([Remainder] string location)
        {
            var result = await _weatherSearchService.GetCurrentWeatherByLocationTextAsync(location, App.WeatherClient.DarkSky);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync("Something went wrong");
                return;
            }

            var description = string.Format("Currently {0} and {1} with a high of {2} and a low of {3}",
                ConvertToTempString(result.Temperature), result.Condition, ConvertToTempString(result.ForecastHigh), ConvertToTempString(result.ForecastLow));

            var embed = new EmbedBuilder()
                .WithAuthor($"{result.City}, {result.Region} - {result.Country}")
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
        public async Task GetFutureWeatherForecastAsync([Remainder] string location)
        {
            var result = await _weatherSearchService.GetForecastWeatherByLocationTextAsync(location, App.WeatherClient.DarkSky);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync("Something went wrong");
                return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithAuthor($"{result.City}, {result.Region} - {result.Country}")
                .WithColor(0x070707);

            foreach(var day in result.Forecast.Take(5))
            {
                embedBuilder.AddField(day.Date, CreateWeatherDay(day), false);
            }

            var embed = embedBuilder.Build();

            await Context.Channel.SendMessageAsync(embed: embed);
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
    }
}
