using dbank.Application.Abstractions;
using dbank.Application.Models.Credits;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/credits")]
public class CreditsController(ICreditsService creditsService) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCreditDto dto)
    {
        await creditsService.Create(dto);
        
        return Ok();
    }

    [HttpGet("{creditId}")]
    public async Task<IActionResult> GetById(long creditId)
    {
        var result = await creditsService.GetById(creditId);
        
        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        var result = await creditsService.GetByUser(customerId);
        
        return Ok(result);
    }
}
