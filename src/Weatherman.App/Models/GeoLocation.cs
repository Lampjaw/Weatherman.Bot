namespace Weatherman.App.Models
{
    public class GeoLocation
    {
        public GeoLocation(float latitude, float longitude, string country, string region, string city)
        {
            Coordinates = new Coordinates(latitude, longitude);
            Country = country;
            Region = region;
            City = city;
        }

        public Coordinates Coordinates { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
    }
}
