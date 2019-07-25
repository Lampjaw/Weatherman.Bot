using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Models;

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

            var weather = await _weatherService.GetCurrentWeatherAsync(place.Coordinates, weatherClient, cancellationToken);

            weather.Country = place.Country;
            weather.Region = place.Region;
            weather.City = place.City;

            return weather;
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocationTextAsync(string location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var place = await _locationService.FindLocationByTextAsync(location, cancellationToken);

            if (place == null)
            {
                return null;
            }

            var weather = await _weatherService.GetForecastWeatherAsync(place.Coordinates, weatherClient, cancellationToken);

            weather.Country = place.Country;
            weather.Region = place.Region;
            weather.City = place.City;

            return weather;
        }
    }
}
