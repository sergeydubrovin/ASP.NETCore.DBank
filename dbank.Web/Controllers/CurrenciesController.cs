using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DBank.Web.Controllers;

[Route("api/currencies")]
public class CurrenciesController(ICurrenciesService currenciesService, ILogger<CurrenciesController> logger,
                                  IOptions<RedisOptions> rOptions) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> GetByCurrencyCode(string currencyCode)
    {
        logger.LogInformation($"Method api/currencies GetByCurrencyCode: {currencyCode} started.");

        if (!rOptions.Value.SupportedCurrencies.Contains(currencyCode))
        {
            logger.LogError($"Method api/currencies GetByCurrencyCode: {currencyCode} failed.");
            return BadRequest("Currency is not supported.");
        }
        
        var result = await currenciesService.GetByCurrencyCode(currencyCode);
        
        logger.LogInformation($"Method api/currencies GetByCurrencyCode completed." +
                              $"Response: {JsonSerializer.Serialize(result)}");
        return Ok(result);
    }
    
    [HttpPost("currency-converter")]
    public async Task<IActionResult> CurrencyConverter(string currencyCode, decimal amountRubles)
    {
        logger.LogInformation($"Method api/currencies CurrencyConverter: {currencyCode} started.");

        if (!rOptions.Value.SupportedCurrencies.Contains(currencyCode))
        {
            logger.LogError($"Method api/currencies CurrencyConverter: {currencyCode} failed.");
            return BadRequest("Currency is not supported.");
        }
        
        var result = await currenciesService.CurrencyConverter(currencyCode, amountRubles);
        
        logger.LogInformation($"Method api/currencies CurrencyConverter completed." +
                              $"Response: {JsonSerializer.Serialize(result)}");
        return Ok(result);
    }
}
