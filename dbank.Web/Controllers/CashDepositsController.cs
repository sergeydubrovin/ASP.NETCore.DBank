using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.CashDeposits;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers;

[Route("api/cash-deposits")]
public class CashDepositsController(ICashDepositsService depositsService, 
                                    ILogger<CashDepositsController> logger) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCashDepositDto dto)
    {
        logger.LogInformation($"Method api/cash-deposits Create started. Request: {JsonSerializer.Serialize(dto)}");
        
        var result = await depositsService.Create(dto);

        logger.LogInformation($"Method api/cash-deposits Create finished. Request: {JsonSerializer.Serialize(dto)}" +
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }

    [HttpGet("{depositId:long}")]
    public async Task<IActionResult> GetById(long depositId)
    {
        logger.LogInformation($"Method api/cash-deposits/{depositId} GetById started.");
        
        var result = await depositsService.GetById(depositId);
        
        logger.LogInformation($"Method api/cash-deposits/{depositId} GetById finished." + 
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }

    [HttpGet("customer/{customerId:long}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        logger.LogInformation($"Method api/cash-deposits/customer/{customerId} GetByUser started.");
        
        var result = await depositsService.GetByUser(customerId);
        
        logger.LogInformation($"Method api/cash-deposits/customer/{customerId} GetByUser finished. " +
                              $"Result count: {result.Count}.");
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        logger.LogInformation("Method api/cash-deposits GetAll started.");
        
        var result = await depositsService.GetAll();
        
        logger.LogInformation($"Method api/cash-deposits GetAll finished. Result count: {result.Count}");
        
        return Ok(result);
    }
}
