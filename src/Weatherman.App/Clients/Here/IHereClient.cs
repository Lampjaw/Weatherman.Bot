using System;
using System.Threading;
using System.Threading.Tasks;
using Weatherman.Domain.Models;

namespace Weatherman.App.Clients.Here
{
    public interface IHereClient : IDisposable
    {
        Task<GeoLocation> GetLocationByTextAsync(string location, CancellationToken cancellationToken);
    }
}