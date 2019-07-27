using System.Threading;
using System.Threading.Tasks;
using Weatherman.Domain;
using Weatherman.Domain.Models;

namespace Weatherman.App.Services
{
    public class WeatherSearchService : IWeatherSearchService
    {
        private readonly ILocationService _locationService;
        private readonly IWeatherService _weatherService;

        public WeatherSearchService(ILocationService locationService, IWeatherService weatherService)
        {
            _locationService = locationService;
            _weatherService = weatherService;
        }

        public async Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(string location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var place = await _locationService.FindLocationByTextAsync(location, cancellationToken);

            if (place == null)
            {
                return null;
            }

            return await GetCurrentWeatherByLocationTextAsync(place, weatherClient, cancellationToken);
        }

        public async Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(GeoLocation location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var weather = await _weatherService.GetCurrentWeatherAsync(location.Coordinates, weatherClient, cancellationToken);
            weather.Location = location;
            return weather;
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocationTextAsync(string location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var place = await _locationService.FindLocationByTextAsync(location, cancellationToken);

            if (place == null)
            {
                return null;
            }

            return await GetForecastWeatherByLocationTextAsync(place, weatherClient, cancellationToken);
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocationTextAsync(GeoLocation location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var weather = await _weatherService.GetForecastWeatherAsync(location.Coordinates, weatherClient, cancellationToken);

            weather.Location = location;

            return weather;
        }
    }
}
