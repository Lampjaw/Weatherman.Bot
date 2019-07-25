using System;
using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Models;

namespace Weatherman.App.Clients.YahooWeather
{
    public interface IYahooWeatherClient : IDisposable
    {
        Task<CurrentWeather> GetWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken);
        Task<ForecastWeather> GetForecastWeatherByLocationAsync(Coordinates coords, CancellationToken cancellationToken);
    }
}