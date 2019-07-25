﻿using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Models;

namespace Weatherman.App.Services
{
    public interface IWeatherService
    {
        Task<CurrentWeather> GetCurrentWeatherAsync(Coordinates coords, WeatherClient clientType = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
        Task<ForecastWeather> GetForecastWeatherAsync(Coordinates coords, WeatherClient clientType = WeatherClient.Yahoo, CancellationToken cancellationToken = default(CancellationToken));
    }
}