using System.Text.Json;
using dbank.Application.Abstractions;
using dbank.Application.Models.Payments;
using Microsoft.AspNetCore.Mvc;

namespace dbank.Web.Controllers
{
    [Route("api/payments")]
    public class PaymentsController(IPaymentsService paymentsService, 
                                    ILogger<PaymentsController> logger) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            logger.LogInformation($"Method api/payments Create started. Request: {JsonSerializer.Serialize(dto)}");
            
            await paymentsService.Create(dto);

            logger.LogInformation($"Method api/payments Create finished. Request: {JsonSerializer.Serialize(dto)}" +
                                  $"Response: {JsonSerializer.Serialize(dto)}");;
            
            return Ok();
        }

        [HttpGet("{paymentId:long}")]
        public async Task<IActionResult> GetById(long paymentId)
        {
            logger.LogInformation($"Method api/payments/{paymentId} GetById started.");
            
            var result = await paymentsService.GetById(paymentId);

            logger.LogInformation($"Method api/payments/{paymentId} GetById finished." +
                                  $"Response: {JsonSerializer.Serialize(result)}");
            
            return Ok(result);
        }

        [HttpGet("customer/{customerId:long}")]
        public async Task<IActionResult> GetByUser(long customerId)
        {
            logger.LogInformation($"Method api/payments/customer/{customerId} GetByUser started.");
            
            var result = await paymentsService.GetByUser(customerId);

            logger.LogInformation($"Method api/payments/customer/{customerId} GetByUser finished." +
                                  $"Result count: {result.Count}");
            
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Method api/payments GetAll started.");
            
            var result = await paymentsService.GetAll();
            
            logger.LogInformation($"Method api/payments GetAll finished. Result count: {result.Count}");
            
            return Ok(result);
        }
    }
}
