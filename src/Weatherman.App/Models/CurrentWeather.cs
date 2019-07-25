namespace Weatherman.App.Models
{
    public class CurrentWeather
    {
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public double Humidity { get; set; }
        public double WindChill { get; set; }
        public double WindSpeed { get; set; }
        public double ForecastHigh { get; set; }
        public double ForecastLow { get; set; }
        public double HeatIndex { get; set; }
        public string Icon { get; set; }
        public GeoLocation Location { get; set; }
    }
}
