namespace Weatherman.App.Models
{
    public class Coordinates
    {
        public Coordinates(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
