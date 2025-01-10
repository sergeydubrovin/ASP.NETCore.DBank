using System.Text.Json;
using dbank.Application.Abstractions;
using dbank.Application.Models.Balances;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/balances")]
public class BalancesController(IBalancesService balancesService,
                                ILogger<BalancesController> logger) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBalanceDto dto)
    {
        logger.LogInformation($"Method api/balances Create started. Request: {JsonSerializer.Serialize(dto)}");
        
        await balancesService.Create(dto);

        logger.LogInformation($"Method api/balances Create finished. Request: {JsonSerializer.Serialize(dto)}" +
                              $"Response: {JsonSerializer.Serialize(dto)}");
        
        return Ok();
    }

    [HttpGet("customer/{customerId:long}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        logger.LogInformation($"Method api/balances/customer/{customerId} GetByUser started.");
        
        var result = await balancesService.GetByUser(customerId);

        logger.LogInformation($"Method api/balances/customer/{customerId} GetByUser finished.");
        
        return Ok(result);
    }
}
