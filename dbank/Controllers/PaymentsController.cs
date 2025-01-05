using dbank.Application.Abstractions;
using dbank.Application.Models.Payments;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers
{
    [Route("api/payments")]
    public class PaymentsController(IPaymentsService paymentsService) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            await paymentsService.Create(dto);

            return Ok();
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetById(long paymentId)
        {
            var result = await paymentsService.GetById(paymentId);

            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByUser(long customerId)
        {
            var result = await paymentsService.GetByUser(customerId);

            return Ok(result);
        }
    }
}
