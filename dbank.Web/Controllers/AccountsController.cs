using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers;

[Route("api/accounts")]
public class AccountsController(IIdentityService identity, ILogger<AccountsController> logger) : ApiBaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        logger.LogInformation($"Method api/accounts/login Login started. Request: {JsonSerializer.Serialize(userLoginDto)}");
        
        var result = await identity.Login(userLoginDto);

        logger.LogInformation($"Method api/accounts Login completed. Request: {JsonSerializer.Serialize(userLoginDto)} " +
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        logger.LogInformation($"Method api/accounts/register Register started. Request: {JsonSerializer.Serialize(userRegisterDto)}");
        
        var result = await identity.Register(userRegisterDto);
        
        logger.LogInformation($"Method api/accounts/register Register completed. Request: {JsonSerializer.Serialize(userRegisterDto)} " +
                              $"Response: {JsonSerializer.Serialize(result)}");
        
        return Ok(result);
    }
}
