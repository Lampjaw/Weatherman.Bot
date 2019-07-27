using System.Threading.Tasks;
using Weatherman.App.Services;
using Weatherman.Domain;
using Weatherman.Domain.Exceptions;
using Weatherman.Domain.Models;

namespace Weatherman.Discord
{
    public class WeatherLookupService : IWeatherLookupService
    {
        private readonly IWeatherSearchService _weatherSearchService;
        private readonly IUserManager _userManager;

        public WeatherLookupService(IWeatherSearchService weatherSearchService, IUserManager userManager)
        {
            _weatherSearchService = weatherSearchService;
            _userManager = userManager;
        }

        public async Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(string userId, string locationText = null, WeatherClient weatherClient = WeatherClient.DarkSky)
        {
            if (string.IsNullOrWhiteSpace(locationText))
            {
                var userLocation = await GetPersistentUserLocationAsync(userId);

                return await _weatherSearchService.GetCurrentWeatherByLocationTextAsync(userLocation, weatherClient);
            }

            var result = await _weatherSearchService.GetCurrentWeatherByLocationTextAsync(locationText, weatherClient);

            if (result != null)
            {
                await _userManager.UpdateUserLastLocationAsync(userId, result.Location);
            }

            return result;
        }

        public async Task<ForecastWeather> GetFutureWeatherByLocationTextAsync(string userId, string locationText = null, WeatherClient weatherClient = WeatherClient.DarkSky)
        {
            if (string.IsNullOrWhiteSpace(locationText))
            {
                var userLocation = await GetPersistentUserLocationAsync(userId);

                return await _weatherSearchService.GetForecastWeatherByLocationTextAsync(userLocation, weatherClient);
            }

            var result = await _weatherSearchService.GetForecastWeatherByLocationTextAsync(locationText, weatherClient);

            if (result != null)
            {
                await _userManager.UpdateUserLastLocationAsync(userId, result.Location);
            }

            return result;
        }

        private async Task<GeoLocation> GetPersistentUserLocationAsync(string userId)
        {
            var userLocation = await _userManager.GetUsersDefaultLocationAsync(userId);
            if (userLocation != null)
            {
                return userLocation;
            }

            throw new MissingUserLocationException();
        }
    }
}
