using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers;

[Route("api/customers")]
public class CustomersController(ICustomersService customersService,
                                 ILogger<CustomersController> logger) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        logger.LogInformation($"Method api/customers Create started. Request: {JsonSerializer.Serialize(dto)}");
        
        await customersService.Create(dto);
        
        logger.LogInformation($"Method api/customers Create finished. Request: {JsonSerializer.Serialize(dto)}" +
                              $"Response: {JsonSerializer.Serialize(dto)}");
        
        return Ok();
    }

    [HttpGet("{customerId:long}")]
    public async Task<IActionResult> GetById(long customerId)
    {
         logger.LogInformation($"Method api/customers/{customerId} GetById started.");
        
         var result = await customersService.GetById(customerId);

         logger.LogInformation($"Method api/customers/{customerId} GetById finished." + 
                               $"Response: {JsonSerializer.Serialize(result)}");
         
         return Ok(result);
    }
}
