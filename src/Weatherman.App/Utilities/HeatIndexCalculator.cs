using System;

namespace Weatherman.App.Utilities
{
    internal static class HeatIndexCalculator
    {
        private static double _c1 = -42.379;
        private static double _c2 = 2.04901523;
        private static double _c3 = 10.14333127;
        private static double _c4 = -0.22475541;
        private static double _c5 = -0.00683783;
        private static double _c6 = -0.05481717;
        private static double _c7 = 0.00122874;
        private static double _c8 = 0.00085282;
        private static double _c9 = -0.00000199;

        public static double Calculate(double temperature, double humidity)
        {
            var t = temperature;
            var r = humidity;

            var heatIndex = 0.5 * (t + 61.0 + ((t - 68.0) * 1.2) + (r * 0.094));

            if (heatIndex < 80)
            {
                return heatIndex;
            }

            heatIndex =
                _c1 +
                _c2 * t +
                _c3 * r +
                _c4 * t * r +
                _c5 * t * t +
                _c6 * r * r +
                _c7 * t * t * r +
                _c8 * t * r * r +
                _c9 * t * t * r * r;

            if (r < 13 && t >= 80 && t <= 112)
            {
                return heatIndex - ((13 - r) / 4) * Math.Sqrt((17 - Math.Abs(t - 95)) / 17);
            }

            if (r > 85 && t >= 80 && t <= 87)
            {
                return heatIndex + ((r - 85) / 10) * ((87 - t) / 5);
            }

            return heatIndex;
        }
    }
}
