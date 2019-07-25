﻿using System.Threading;
using System.Threading.Tasks;
using Weatherman.App.Models;

namespace Weatherman.App.Services
{
    public interface ILocationService
    {
        Task<GeoLocation> FindLocationByTextAsync(string text, CancellationToken cancellationToken = default(CancellationToken));
    }
}