using dbank.Application.Abstractions;
using dbank.Application.Models.Balances;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/balances")]
public class BalancesController(IBalancesService balancesService) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBalanceDto dto)
    {
        await balancesService.Create(dto);

        return Ok();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        var result = await balancesService.GetByUser(customerId);
        
        return Ok(result);
    }
}
