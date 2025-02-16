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
        
        return Accepted($"Customer creation initiated. Please verify your email. Your id: {userId}");
    }

    [HttpPost("verification")]
    public async Task<IActionResult> CompleteVerification(VerificationDto verification)
    {
        try
        {
            logger.LogInformation("Method api/customers/save CompleteVerification started.");
            
            await customersService.CompleteVerification(verification);
            
            logger.LogInformation("Method api/customers/save CompleteVerification completed.");
            
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError($"Method api/customers/save CompleteVerification failed. Error: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("resend")]
    public async Task<IActionResult> ResendCode(long customerId)
    {
        logger.LogInformation($"Method api/customers/resend Resend started. CustomerId: {customerId}");
        
        await customersService.ResendCode(customerId);
        
        logger.LogInformation("Method api/customers/resend Resend completed.");
        
        return Ok();
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
