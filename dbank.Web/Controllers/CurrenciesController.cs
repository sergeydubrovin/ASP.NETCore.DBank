using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DBank.Web.Controllers;

[Route("api/currencies")]
public class CurrenciesController(ICurrenciesService currenciesService, ILogger<CurrenciesController> logger,
                                  IOptions<CbOptions> cbOptions) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> GetByCurrencyCode(string currencyCode)
    {
        logger.LogInformation($"Method api/currencies GetByCurrencyCode: {currencyCode} started.");

        if (!cbOptions.Value.SupportedCurrencies.Contains(currencyCode))
        {
            logger.LogInformation($"Method api/currencies GetByCurrencyCode: {currencyCode} failed.");
            return BadRequest("Currency is not supported.");
        }
        
        var result = await currenciesService.GetByCurrencyCode(currencyCode);
        
        logger.LogInformation($"Method api/currencies GetByCurrencyCode finished." +
                              $"Response: {JsonSerializer.Serialize(result)}");
        return Ok(result);
    }
}
