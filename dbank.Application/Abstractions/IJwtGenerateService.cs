using System.Security.Claims;
using DBank.Application.Models.Identity;

namespace DBank.Application.Abstractions;

public interface IJwtGenerateService
{
    string GenerateToken(UserResponse userRegisterModel, ClaimsIdentity claimsIdentity);
}
