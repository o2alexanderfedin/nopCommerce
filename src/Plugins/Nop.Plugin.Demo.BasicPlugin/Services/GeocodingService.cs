using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Services.Directory;

namespace Nop.Plugin.Demo.BasicPlugin.Services;

public class GeocodingService : IGeocodingService
{
    private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly IStateProvinceService _stateProvinceService;
    private readonly ICountryService _countryService;
    private const string CacheKeyPrefix = "Geocoding:";

    public GeocodingService(
        IMemoryCache cache,
        IStateProvinceService stateProvinceService,
        ICountryService countryService)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "NopCommerce/4.5");
        _cache = cache;
        _stateProvinceService = stateProvinceService;
        _countryService = countryService;
    }

    public async Task<GeoCoordinate?> GeocodeAddressAsync(Address address)
    {
        if (!CanGeocodeAddress(address))
            return null;

        var stateProvince = await _stateProvinceService.GetStateProvinceByIdAsync(address.StateProvinceId ?? 0);
        var country = await _countryService.GetCountryByIdAsync(address.CountryId ?? 0);

        // Build query in standard postal address format:
        // Street Address, City, State/Province Postal Code, Country
        var query = address.Address1;
        
        if (address is {City: { Length: > 0 } addressCity})
            query += $", {addressCity}";
        
        if (stateProvince is {Name: { Length: > 0 } stateProvinceName})
            query += $", {stateProvinceName}";

        if (country is {Name: { Length: > 0 } countryName})
            query += $", {countryName}";

        if (address is {ZipPostalCode: { Length: > 0 } zipPostalCode})
            query += $" {zipPostalCode}";

        var encodedQuery = Uri.EscapeDataString(query);
        var url = $"{GeocodingApiUrl}?q={encodedQuery}&format=json&limit=1";

        try
        {
            var response = await _httpClient.GetStringAsync(url);
            var results = JsonSerializer.Deserialize<GeocodingResult[]>(response);

            if (results == null || results.Length == 0)
                return null;

            var result = results[0];
            return new GeoCoordinate(decimal.Parse(result.Lat), decimal.Parse(result.Lon));
        }
        catch
        {
            return null;
        }
    }

    public async Task<GeoCoordinate?> TryGeocodeAddressWithCachingAsync(Address address)
    {
        if (!CanGeocodeAddress(address))
            return null;

        var cacheKey = GetCacheKey(address);
        if (_cache.TryGetValue<GeoCoordinate>(cacheKey, out var cachedCoordinate))
            return cachedCoordinate;

        var coordinate = await GeocodeAddressAsync(address);
        if (coordinate != null)
        {
            _cache.Set(cacheKey, coordinate, TimeSpan.FromDays(30));
        }

        return coordinate;
    }

    public bool CanGeocodeAddress(Address address)
    {
        // We need at least a street address and city to geocode
        return address is 
        {
            Address1: { Length: > 0 },
            City: { Length: > 0 }
        };
    }

    public Task ClearCacheAsync(Address address)
    {
        if (address is not null)
        {
            var cacheKey = GetCacheKey(address);
            _cache.Remove(cacheKey);
        }
        return Task.CompletedTask;
    }

    private string GetCacheKey(Address address)
    {
        return address switch
        {
            { Address1: var addr1,
              City: var city,
              ZipPostalCode: var zip,
              StateProvinceId: var stateId,
              CountryId: var countryId }
                => $"{CacheKeyPrefix}{addr1}:{city}:{zip}:{stateId}:{countryId}",
            _ => string.Empty
        };
    }

    private class GeocodingResult
    {
        public string Lat { get; set; } = string.Empty;
        public string Lon { get; set; } = string.Empty;
    }
}
