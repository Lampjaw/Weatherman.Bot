using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Models;
using Weatherman.App.Utilities;

namespace Weatherman.App.Clients.DarkSky
{
    internal class DarkSkyClient : IDarkSkyClient
    {
        private const string WEATHER_URL = "https://api.darksky.net";

        private readonly HttpClient _httpClient;
        private readonly DarkSkyOptions _options;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Converters = new JsonConverter[]
                {
                    new BooleanJsonConverter(),
                    new DateTimeJsonConverter()
                }
        };

        public DarkSkyClient(IOptions<DarkSkyOptions> options)
        {
            _options = options.Value;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(WEATHER_URL);
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var result = await GetWeatherFutureAsync(coords, cancellationToken);
            if (result == null)
            {
                return null;
            }

            var forecasts = result.Daily.Data.Select(a => new ForecastWeather.WeatherDay {
                Date = a.Time.ToShortDateString(),
                Day = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames[(int)a.Time.DayOfWeek],
                High = a.TemperatureHigh,
                Low = a.TemperatureLow,
                Text = a.Summary,
                Icon = a.Icon});

            return new ForecastWeather
            {
                Forecast = forecasts
            };
        }

        public async Task<CurrentWeather> GetWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var result = await GetWeatherCurrentAsync(coords, cancellationToken);
            if (result == null)
            {
                return null;
            }

            var currentDay = result.Daily.Data.FirstOrDefault();

            var temp = result.Currently.Temperature;
            var humidity = result.Currently.Humidity;
            var windSpeed = result.Currently.WindSpeed;
            var heatIndex = HeatIndexCalculator.Calculate(temp, humidity);
            var windChill = WindChillCalculator.Calculate(temp, windSpeed);

            return new CurrentWeather
            {
                Condition = result.Currently.Summary,
                Temperature = temp,
                Humidity = humidity,
                WindChill = windChill,
                WindSpeed = windSpeed,
                ForecastHigh = currentDay.TemperatureHigh,
                ForecastLow = currentDay.TemperatureLow,
                HeatIndex = heatIndex,
                Icon = currentDay.Icon
            };
        }

        private async Task<DarkSkyResponse> GetWeatherCurrentAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/forecast/{_options.DarkSkySecretKey}/{coords.Latitude},{coords.Longitude}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.GetContentAsync<DarkSkyResponse>(_serializerSettings);
        }

        private async Task<DarkSkyResponse> GetWeatherFutureAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/forecast/{_options.DarkSkySecretKey}/{coords.Latitude},{coords.Longitude}?exclude=currently,flags,hourly,minutely");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.GetContentAsync<DarkSkyResponse>(_serializerSettings);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
