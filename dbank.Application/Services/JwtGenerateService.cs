using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DBank.Application.Abstractions;
using DBank.Application.Models.Identity;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DBank.Application.Services;

public class JwtGenerateService(IOptions<JwtOptions> jwtOptions) : IJwtGenerateService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public string GenerateToken(UserResponse userRegisterModel, ClaimsIdentity claimsIdentity)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.TokenPrivateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);
        
        var claims = claimsIdentity.Claims.ToDictionary(c => c.Type, object (c) => c.Value);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpireHours),
            SigningCredentials = credentials,
            Claims = claims,
            Audience = _jwtOptions.TokenAudience,
            Issuer = _jwtOptions.TokenIssuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
