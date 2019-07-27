using System;
using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Clients.DarkSky;
using Weatherman.App.Clients.YahooWeather;
using Weatherman.Domain;
using Weatherman.Domain.Models;

namespace Weatherman.App.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IYahooWeatherClient _yahooWeatherClient;
        private readonly IDarkSkyClient _darkSkyClient;

        public WeatherService(IYahooWeatherClient yahooWeatherClient, IDarkSkyClient darkSkyClient)
        {
            _yahooWeatherClient = yahooWeatherClient;
            _darkSkyClient = darkSkyClient;
        }

        public Task<CurrentWeather> GetCurrentWeatherAsync(Coordinates coords, WeatherClient clientType = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch(clientType)
            {
                case WeatherClient.Yahoo:
                    return _yahooWeatherClient.GetWeatherByLocationAsync(coords, cancellationToken);
                case WeatherClient.DarkSky:
                    return _darkSkyClient.GetWeatherByLocationAsync(coords, cancellationToken);
            }

            throw new InvalidOperationException($"Weather client of type '{clientType}' does not have a handler.");
        }

        public Task<ForecastWeather> GetForecastWeatherAsync(Coordinates coords, WeatherClient clientType = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (clientType)
            {
                case WeatherClient.Yahoo:
                    return _yahooWeatherClient.GetForecastWeatherByLocationAsync(coords, cancellationToken);
                case WeatherClient.DarkSky:
                    return _darkSkyClient.GetForecastWeatherByLocationAsync(coords, cancellationToken);
            }

            throw new InvalidOperationException($"Weather client of type '{clientType}' does not have a handler.");
        }
    }
}
