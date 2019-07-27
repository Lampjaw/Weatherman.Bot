using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Weatherman.App.Utilities;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Weatherman.Domain.Models;

namespace Weatherman.App.Clients.YahooWeather
{
    internal class YahooWeatherClient: IYahooWeatherClient
    {
        private const string WEATHER_URL = "https://weather-ydn-yql.media.yahoo.com/forecastrss";

        private readonly HttpOAuthClient _httpClient;
        private readonly YahooWeatherOptions _options;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new UnderscorePropertyNamesContractResolver(),
            Converters = new JsonConverter[]
                {
                    new BooleanJsonConverter(),
                    new DateTimeJsonConverter()
                }
        };

        public YahooWeatherClient(IOptions<YahooWeatherOptions> options)
        {
            _options = options.Value;

            _httpClient = new HttpOAuthClient(_options.YahooClientId, _options.YahooSecret)
            {
                BaseAddress = new Uri(WEATHER_URL)
            };
            _httpClient.DefaultRequestHeaders.Add("Yahoo-App-Id", _options.YahooAppId);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<YahooWeatherResponse> GetWeatherAsync(string location, CancellationToken cancellationToken)
        {
            var loc = HttpUtility.UrlEncode(location, Encoding.UTF8);
            var url = $"?location={loc}&format=json";

            var locationParam = new KeyValuePair<string, string>("location", location);

            var response = await _httpClient.GetAsync(url, cancellationToken, locationParam);
            return await response.GetContentAsync<YahooWeatherResponse>(_serializerSettings);
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var result = await GetWeatherAsync($"{coords.Latitude},{coords.Longitude}", cancellationToken);

            var forecasts = result.Forecasts.Select(a => new ForecastWeather.WeatherDay { Date = a.Date.ToShortDateString(), Day = a.Day, High = a.High, Low = a.Low, Text = a.Text });

            return new ForecastWeather
            {
                Forecast = forecasts
            };
        }

        public async Task<CurrentWeather> GetWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken)
        {
            var result = await GetWeatherAsync($"{coords.Latitude},{coords.Longitude}", cancellationToken);

            var currentDay = result.Forecasts.FirstOrDefault();

            var temp = result.CurrentObservation.Condition.Temperature;
            var humidity = result.CurrentObservation.Atmosphere.Humidity;
            var heatIndex = HeatIndexCalculator.Calculate(temp, humidity);

            return new CurrentWeather
            {
                Condition = result.CurrentObservation.Condition.Text,
                Temperature = temp,
                Humidity = humidity,
                WindChill = result.CurrentObservation.Wind.Chill,
                WindSpeed = result.CurrentObservation.Wind.Speed,
                ForecastHigh = currentDay.High,
                ForecastLow = currentDay.Low,
                HeatIndex = (int)heatIndex
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
