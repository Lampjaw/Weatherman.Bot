using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Weatherman.App.Models;

namespace Weatherman.App.Clients.Here
{
    internal class HereClient : IHereClient
    {
        private const string HERE_URL = "https://geocoder.api.here.com/6.2/geocode.json";
        private readonly HereOptions _options;
        private readonly HttpClient _httpClient;

        public HereClient(IOptions<HereOptions> options)
        {
            _options = options.Value;

            _httpClient = new HttpClient();
        }

        public async Task<GeoLocation> GetLocationByTextAsync(string location, CancellationToken cancellationToken)
        {
            var uri = QueryHelpers.AddQueryString(HERE_URL, CreateQueryStringParts(location));
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.GetContentAsync<HereResponse>();

            var foundPlace = result.Response.View.FirstOrDefault()?.Result.FirstOrDefault()?.Location;

            if (foundPlace == null)
            {
                return null;
            }

            var additionalAddress = foundPlace.Address.AdditionalData.ToDictionary(a => a.Key, a => a.Value);
            var countryName = additionalAddress?.GetValueOrDefault("CountryName") ?? foundPlace.Address.Country;

            return new GeoLocation(foundPlace.DisplayPosition.Latitude, foundPlace.DisplayPosition.Longitude, countryName, foundPlace.Address.State, foundPlace.Address.City);
        }

        private Dictionary<string, string> CreateQueryStringParts(string searchQuery)
        {
            return new Dictionary<string, string>
            {
                { "app_id", _options.HereAppId },
                { "app_code", _options.HereAppCode },
                { "searchtext", HttpUtility.UrlEncode(searchQuery) }
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
