using System;

namespace Weatherman.App.Utilities
{
    internal static class WindChillCalculator
    {
        private static double _c1 = 35.74;
        private static double _c2 = 0.6215;
        private static double _c3 = 33.75;
        private static double _c4 = 0.4275;

        public static double Calculate(double temperature, double windSpeed)
        {
            var ws = Math.Pow(windSpeed, 0.16);
            return _c1 + (_c2 * temperature) - (_c3 * ws) + (_c4 * temperature * ws);
        }
    }
}
