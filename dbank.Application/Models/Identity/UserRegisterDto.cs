using System.Text.Json.Serialization;

namespace DBank.Application.Models.Identity;

public class UserRegisterDto
{
    public required string Card { get; set; }
    public required string Password { get; set; }

    [JsonIgnore] public string? UserName { get; set; }
}
