using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers;

[Route("api/customers")]
public class CustomersController(ICustomersService customersService,
                                 ILogger<CustomersController> logger) : ApiBaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        logger.LogInformation($"Method api/customers Create started. Request: {JsonSerializer.Serialize(dto)}");
        
        var userId = await customersService.Create(dto);
        
        logger.LogInformation($"Method api/customers Create completed. Response: {JsonSerializer.Serialize(userId)}");
        
        return Accepted($"Customer creation initiated. Please verify your email. Your key: {userId}");
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(VerificationDto verification)
    {
        try
        {
            logger.LogInformation("Method api/customers/save Save started.");
            
            await customersService.Save(verification);
            
            logger.LogInformation("Method api/customers/save Save completed.");
            
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError($"Method api/customers/save Save failed. Error: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{customerId:long}")]
    public async Task<IActionResult> GetById(long customerId)
    {
         logger.LogInformation($"Method api/customers/{customerId} GetById started.");
        
         var result = await customersService.GetById(customerId);

         logger.LogInformation($"Method api/customers/{customerId} GetById completed." + 
                               $"Response: {JsonSerializer.Serialize(result)}");
         return Ok(result);
    }
}
