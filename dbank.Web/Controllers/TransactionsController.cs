using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers
{
    [Route("api/transactions")]
    public class TransactionsController(ITransactionsService transactionsService, 
                                        ILogger<TransactionsController> logger) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionsDto dto)
        {
            logger.LogInformation($"Method api/transactions Create started. Request: {JsonSerializer.Serialize(dto)}");
            
            await transactionsService.Create(dto);

            logger.LogInformation($"Method api/transactions Create completed. Request: {JsonSerializer.Serialize(dto)}" +
                                  $"Response: {JsonSerializer.Serialize(dto)}");
            
            return Ok();
        }

        [HttpGet("{transactionsId:long}")]
        public async Task<IActionResult> GetById(long transactionsId)
        {
            logger.LogInformation($"Method api/transactions/{transactionsId} GetById started.");
            
            var result = await transactionsService.GetById(transactionsId);

            logger.LogInformation($"Method api/transactions/{transactionsId} GetById completed." +
                                  $"Response: {JsonSerializer.Serialize(result)}");
            
            return Ok(result);
        }

        [HttpGet("customer/{customerId:long}")]
        public async Task<IActionResult> GetByUser(long customerId)
        {
            logger.LogInformation($"Method api/transactions/customer/{customerId} GetByUser started.");
            
            var result = await transactionsService.GetByUser(customerId);

            logger.LogInformation($"Method api/transactions/customer/{customerId} GetByUser completed." +
                                  $"Result count: {result.Count}");
            
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Method api/transactions GetAll started.");
            
            var result = await transactionsService.GetAll();
            
            logger.LogInformation($"Method api/transactions GetAll completed. Result count: {result.Count}");
            
            return Ok(result);
        }
    }
}
