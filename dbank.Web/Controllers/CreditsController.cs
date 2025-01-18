using System.Text.Json;
using dbank.Application.Abstractions;
using dbank.Application.Models.Credits;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/credits")]
public class CreditsController(ICreditsService creditsService, 
                               ILogger<CreditsController> logger) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCreditDto dto)
    {
        logger.LogInformation($"Method api/credits Create started. Request: {JsonSerializer.Serialize(dto)}");
        
        var result = await creditsService.Create(dto);
        
        logger.LogInformation($"Method api/credits Create finished. Request: {JsonSerializer.Serialize(dto)}" +
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }

    [HttpGet("{creditId:long}")]
    public async Task<IActionResult> GetById(long creditId)
    {
        logger.LogInformation($"Method api/credits/{creditId} GetById started.");
        
        var result = await creditsService.GetById(creditId);
        
        logger.LogInformation($"Method api/credits/{creditId} GetById finished." + 
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }

    [HttpGet("customer/{customerId:long}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        logger.LogInformation($"Method api/credits/customer/{customerId} GetByUser started.");
        
        var result = await creditsService.GetByUser(customerId);
        
        logger.LogInformation($"Method api/credits/customer/{customerId} GetByUser finished. " +
                              $"Result count: {result.Count}.");
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        logger.LogInformation("Method api/credits GetAll started.");
        
        var result = await creditsService.GetAll();
        
        logger.LogInformation($"Method api/credits GetAll finished. Result count: {result.Count}.");
        
        return Ok(result);
    }
}
