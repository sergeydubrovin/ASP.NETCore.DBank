using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Currencies;
using DBank.Domain.Exceptions;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class CurrenciesImporter(IOptions<CbOptions> cbOptions, IHttpClientFactory clientFactory) : ICurrenciesImporter
{
    private readonly CbOptions _cb = cbOptions.Value;
    private readonly HttpClient _client = clientFactory.CreateClient("Cb");
    
    public async Task<ValuteDto> Import()
    {
        try
        {
            var response = await _client.GetAsync(_cb.CurrencyPath);
            var stringContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var deserializeResponse = JsonSerializer.Deserialize<CbCurrencyResponse>(stringContent);

                return deserializeResponse?.Valute!;
            }

            throw new Exception($"Import currencies failed. Error: {stringContent} Status Code: {response.StatusCode}");
        }
        catch(Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }
}
