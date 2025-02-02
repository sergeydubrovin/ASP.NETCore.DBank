using System.Text.Json.Serialization;

namespace DBank.Application.Models.Identity;

public class UserLoginDto
{
    public required string Card { get; set; }
    public required string Password { get; set; }

    [JsonIgnore] public string? Username { get; set; }
}
