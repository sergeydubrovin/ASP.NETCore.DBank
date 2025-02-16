using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DBank.Application.Models.Identity;
using DBank.Application.Services;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace UnitTestApplication;

public class JwtGenerateTest
{
    [Fact]
    public void JwtGenerate_Success_ReturnsToken()
    {
        // Arrange
        var jwtOptions = new JwtOptions
        {
            TokenPrivateKey = "super-secret-key-for-testing-only",
            ExpireHours = 1,
            TokenAudience = "audience",
            TokenIssuer = "issuer"
        };

        var mockOptions = new Mock<IOptions<JwtOptions>>();
        mockOptions.Setup(x => x.Value).Returns(jwtOptions);
        
        var service = new JwtGenerateService(mockOptions.Object);
        var response = new UserResponse
        {
            Id = 1,
            Card = "1234123412341234"
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
            new Claim(ClaimTypes.Name, response.Card),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("issuer", jwtOptions.TokenIssuer),
            new Claim("audience", jwtOptions.TokenAudience),
        };
        
        var claimsIdentity = new ClaimsIdentity(claims);
        
        var tokenValidationOptions = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.TokenPrivateKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.TokenIssuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.TokenAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        // Act
        var token = service.GenerateToken(response, claimsIdentity);
        
        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);

        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(token));
        
        var principal = handler.ValidateToken(token, tokenValidationOptions, out _);
            
        Assert.NotNull(principal);
        Assert.Equal(jwtOptions.TokenIssuer, principal.FindFirstValue("issuer"));
        Assert.Equal(jwtOptions.TokenAudience, principal.FindFirstValue("audience"));
        Assert.Equal(response.Id.ToString(), principal.FindFirstValue(ClaimTypes.NameIdentifier));
        Assert.Equal(response.Card, principal.FindFirstValue(ClaimTypes.Name));
    }
}
