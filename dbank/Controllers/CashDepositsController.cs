using dbank.Application.Abstractions;
using dbank.Application.Models.CashDeposits;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/cash-deposits")]
public class CashDepositsController(ICashDepositsService depositsService) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCashDepositDto dto)
    {
        await depositsService.Create(dto);

        return Ok();
    }

    [HttpGet("{depositId}")]
    public async Task<IActionResult> GetById(long depositId)
    {
        var result = await depositsService.GetById(depositId);
        
        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        var result = await depositsService.GetByUser(customerId);
        
        return Ok(result);
    }
}
