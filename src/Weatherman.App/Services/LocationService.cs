using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Clients.Here;
using Weatherman.App.Models;

namespace Weatherman.App.Services
{
    public class LocationService : ILocationService
    {
        private readonly IHereClient _hereClient;

        public LocationService(IHereClient hereClient)
        {
            _hereClient = hereClient;
        }

        public Task<GeoLocation> FindLocationByTextAsync(string text, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _hereClient.GetLocationByTextAsync(text, cancellationToken);
        }
    }
}
