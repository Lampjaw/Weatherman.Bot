using System.Threading;
using System.Threading.Tasks;
using Weatherman.Domain;
using Weatherman.Domain.Models;

namespace Weatherman.App.Services
{
    public interface IWeatherSearchService
    {
        Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(string location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
        Task<ForecastWeather> GetForecastWeatherByLocationTextAsync(string location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
        Task<ForecastWeather> GetForecastWeatherByLocationTextAsync(GeoLocation location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
        Task<CurrentWeather> GetCurrentWeatherByLocationTextAsync(GeoLocation location, WeatherClient weatherClient = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
    }
}