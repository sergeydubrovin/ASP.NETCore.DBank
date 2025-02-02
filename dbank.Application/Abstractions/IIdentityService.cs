using System.Security.Claims;
using DBank.Application.Models.Identity;

namespace DBank.Application.Abstractions;

public interface IIdentityService
{
    Task<UserResponse> Register(UserRegisterDto userRegisterDto);
    Task<UserResponse> Login(UserLoginDto userLoginDto);
    ClaimsIdentity GenerateClaimsIdentity(UserResponse userRegisterModel);
}
