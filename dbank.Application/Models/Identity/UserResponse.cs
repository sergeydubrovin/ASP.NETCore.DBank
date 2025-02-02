using System.Text.Json.Serialization;

namespace DBank.Application.Models.Identity;

public class UserResponse
{
    public long Id { get; set; }
    public string[]? Roles { get; set; }
    public string? Card { get; set; }
    public string? JwtToken { get; set; }

    [JsonIgnore] public string? UserName { get; set; }
}
