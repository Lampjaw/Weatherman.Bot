using System;
using System.Collections.Generic;

namespace Weatherman.App.Clients.DarkSky
{
    internal class DarkSkyResponse
    {
        public string Timezone { get; set; }
        public DarkSkyCurrently Currently { get; set; }
        public DarkSkyDaily Daily { get; set; }
        public IEnumerable<DarkSkyAlert> Alerts { get; set; }

        internal sealed class DarkSkyCurrently
        {
            public DateTime Time { get; set; }
            public string Summary { get; set; }
            public string Icon { get; set; }
            public double Temperature { get; set; }
            public double ApparentTemperature { get; set; }
            public double Humidity { get; set; }
            public double WindSpeed { get; set; }
        }

        internal sealed class DarkSkyDaily
        {
            public IEnumerable<DarkSkyDailyData> Data { get; set; }

            internal sealed class DarkSkyDailyData
            {
                public DateTime Time { get; set; }
                public string Summary { get; set; }
                public string Icon { get; set; }
                public double TemperatureLow { get; set; }
                public double TemperatureHigh { get; set; }
            }
        }

        internal sealed class DarkSkyAlert
        {
            public string Title { get; set; }
            public DateTime Time { get; set; }
            public DateTime Expires { get; set; }
            public string Description { get; set; }
            public string Uri { get; set; }
        }
    }
}
