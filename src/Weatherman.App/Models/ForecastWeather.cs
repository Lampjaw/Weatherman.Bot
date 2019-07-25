using System.Collections.Generic;

namespace Weatherman.App.Models
{
    public class ForecastWeather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public IEnumerable<WeatherDay> Forecast { get; set; }

        public sealed class WeatherDay
        {
            public string Date { get; set; }
            public string Day { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public string Text { get; set; }
            public string Icon { get; set; }
        }
    }
}
