using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Currencies;
using DBank.Domain.Exceptions;
using DBank.Domain.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class CurrenciesService(IOptions<CbOptions> cbOptions, ICurrenciesImporter currenciesImporter,
                               IDistributedCache cache) : ICurrenciesService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cbOptions.Value.CacheDurationHours)
    };

    public async Task UpdateCurrenciesCache()
    {
        var currencies = await currenciesImporter.Import();
        
        foreach (var property in currencies.GetType().GetProperties())
        {
            var currency = property.GetValue(currencies);
            var currencyJson = JsonSerializer.Serialize(currency);
            
            await cache.SetStringAsync(property.Name, currencyJson, _cacheOptions);
        }
    }
    
    public async Task<ValuteItemDto> GetByCurrencyCode(string currencyCode)
    {
        try
        {
            var currencyJson = await cache.GetStringAsync(currencyCode);

            if (currencyJson == null)
            {
                await UpdateCurrenciesCache();
                currencyJson = await cache.GetStringAsync(currencyCode);
            }
        
            var currency = JsonSerializer.Deserialize<ValuteItemDto>(currencyJson!);
            return currency!;
        }
        catch (Exception ex)
        {
            throw new CurrencyException($"{ex.Message}");
        }
    }
}
