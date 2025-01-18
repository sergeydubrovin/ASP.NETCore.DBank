using System.Text.Json;
using dbank.Application.Abstractions;
using dbank.Application.Extensions;
using dbank.Application.Models.Сurrencies;
using dbank.Domain.Exceptions;
using dbank.Domain.Options;
using Microsoft.Extensions.Options;

namespace dbank.Application.Services;

public class CurrencyImporter(IOptions<CbOptions> cbOptions, IHttpClientFactory clientFactory) : ICurrencyImporter
{
    private readonly CbOptions _cb = cbOptions.Value;
    private readonly HttpClient _client = clientFactory.CreateClient("Cb");
    
    public async Task<ValuteItemDto> ImportByCurrencyCode(string currencyCode)
    {
        var currencies = await Import();
 
        return currencies.SearchCurrency(currencyCode);
    }
    
    public async Task<ValuteDto> Import()
    {
        var response = await _client.GetAsync(_cb.CurrencyPath);
        var stringContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var deserializeResponse = JsonSerializer.Deserialize<CbCurrencyResponse>(stringContent);

            return deserializeResponse?.Valute!;
        }
        
        throw new CurrencyException($"Import currencies failed. Error: {stringContent} Status Code: {response.StatusCode}");
    }
}
