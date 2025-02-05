using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Extensions;
using DBank.Application.Models.Currencies;
using DBank.Domain.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class CurrenciesService(IOptions<RedisOptions> redisOptions, ICurrenciesImporter currenciesImporter,
                               IDistributedCache cache) : ICurrenciesService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(redisOptions.Value.CacheLifeTimeHours)
    };

    public async Task RefreshCurrencyCache()
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
                await RefreshCurrencyCache();
                currencyJson = await cache.GetStringAsync(currencyCode);
            }
        
            var currency = JsonSerializer.Deserialize<ValuteItemDto>(currencyJson!);
            return currency!;
        }
        catch (Exception ex)
        {
            throw new Exception($"To get data by currency code failed. Error: {ex.Message}");
        }
    }
    
    public async Task<CurrencyConvertResponse> CurrencyConverter(string currencyCode, decimal amountRubles)
    {
        try
        {
            var currencyJson = await cache.GetStringAsync(currencyCode);

            if (currencyJson == null)
            {
                await RefreshCurrencyCache();
                currencyJson = await cache.GetStringAsync(currencyCode);
            }
            
            var currency = JsonSerializer.Deserialize<ValuteItemDto>(currencyJson!);
            return currency!.ComputeCurrency(amountRubles);
        }
        catch (Exception ex)
        {
            throw new Exception($"Currency converter failed. Error: {ex.Message}");
        }
    }
}
