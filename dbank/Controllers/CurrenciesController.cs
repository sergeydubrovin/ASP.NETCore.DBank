using System.Text.Json;
using dbank.Application.Abstractions;
using dbank.Application.Models.Сurrencies;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/currencies")]
public class CurrenciesController(ICurrenciesService currenciesService,
                                  ILogger<CurrenciesController> logger) : ApiBaseController
{
    [HttpPut]
    public async Task<IActionResult> Update(long currencyId, CreateCurrencyDto dto)
    {
        logger.LogInformation($"Method api/currencies/{currencyId} started. Request: {JsonSerializer.Serialize(dto)}");
        
        await currenciesService.Update(currencyId, dto);
        
        logger.LogInformation($"Method api/currencies/{currencyId} finished. Request: {JsonSerializer.Serialize(dto)}" +
                              $"Response: {JsonSerializer.Serialize(dto)}");
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        logger.LogInformation("Method api/currencies GetAll started.");
        
        var result = await currenciesService.GetAll();
        
        logger.LogInformation("Method api/currencies GetAll finished.");
        
        return Ok(result);
    }
}
