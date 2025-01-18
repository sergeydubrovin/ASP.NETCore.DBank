using dbank.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers;

[Route("api/currencies")]
public class CurrenciesController(ICurrencyImporter currencyImporter) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> ImportByCurrencyCode(string currencyCode)
    {
        var result = await currencyImporter.ImportByCurrencyCode(currencyCode);
        
        return Ok(result);
    }
}
