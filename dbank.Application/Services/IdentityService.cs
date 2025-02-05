using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using DBank.Application.Abstractions;
using DBank.Application.Models.Identity;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using DBank.Domain.Models;
using DBank.Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class IdentityService(UserManager<UserEntity> userManager, IJwtGenerateService jwtGenerateService,
                             IOptions<JwtOptions> jwtOptions) : IIdentityService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public async Task<UserResponse> Register(UserRegisterDto userRegisterDto)
    {
        if (await userManager.FindByNameAsync(userRegisterDto.Card) != null)
        {
            throw new DuplicateEntityException($"The card number {userRegisterDto.Card} is already exists.");
        }

        var createUserResult = await userManager.CreateAsync(new UserEntity
        {
            Card = userRegisterDto.Card,
            UserName = userRegisterDto.Card,
        }, userRegisterDto.Password);
        
        if (createUserResult.Succeeded)
        {
            var user = await userManager.FindByNameAsync(userRegisterDto.Card);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with card number {userRegisterDto.Card} not registered.");
            }

            var result = await userManager.AddToRoleAsync(user, RoleСonstants.User);

            if (result.Succeeded)
            {
                var userRole = await userManager.GetRolesAsync(user);
                var response = new UserResponse
                {
                    Id = user.Id,
                    Card = user.Card,
                    Roles = userRole.ToArray()
                };

                var claimsIdentity = GenerateClaimsIdentity(response);
                response.JwtToken = jwtGenerateService.GenerateToken(response, claimsIdentity);
                
                return response;
            }
            throw new Exception($"Errors: {string.Join(';', result.Errors.Select(x => $"{x.Code}: {x.Description}"))}");
        }
        throw new Exception("An unexpected situation during authorization occurred.");
    }
    
    public async Task<UserResponse> Login(UserLoginDto userLoginDto)
    {
        var user = await userManager.FindByNameAsync(userLoginDto.Card);

        if (user == null)
        {
            throw new EntityNotFoundException($"User with card number {userLoginDto.Card} not found.");
        }
        
        var checkPasswordResult = await userManager.CheckPasswordAsync(user, userLoginDto.Password);
        
        if (checkPasswordResult)
        {
            var userRole = await userManager.GetRolesAsync(user);
            var response = new UserResponse
            {
                Id = user.Id,
                Card = user.Card,
                Roles = userRole.ToArray()
            };
            var claimsIdentity = GenerateClaimsIdentity(response);
            response.JwtToken = jwtGenerateService.GenerateToken(response, claimsIdentity);
            
            return response;
        }
        throw new AuthenticationException();
    }
    
    public ClaimsIdentity GenerateClaimsIdentity(UserResponse userRegisterModel)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, userRegisterModel.Card!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.TokenIssuer));

        foreach (var role in userRegisterModel.Roles!)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
}
