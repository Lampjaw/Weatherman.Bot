using System.Threading.Tasks;
using Weatherman.Domain;
using Weatherman.Domain.Models;

namespace Weatherman.Discord
{
    public interface IWeatherLookupService
    {
        Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(string userId, string locationText = null, WeatherClient weatherClient = WeatherClient.DarkSky);
        Task<ForecastWeather> GetFutureWeatherByLocationTextAsync(string userId, string locationText = null, WeatherClient weatherClient = WeatherClient.DarkSky);
    }
}