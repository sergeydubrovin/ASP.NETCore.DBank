using dbank.Application.Abstractions;
using dbank.Application.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/customers")]
public class CustomersController(ICustomersService customersService) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        await customersService.Create(dto);
        
        return Ok();
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(long customerId)
    {
         var result = await customersService.GetById(customerId);

         return Ok(result);
    }
}
