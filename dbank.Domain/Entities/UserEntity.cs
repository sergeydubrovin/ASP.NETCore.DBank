using Microsoft.AspNetCore.Identity;

namespace DBank.Domain.Entities;

public class UserEntity : IdentityUser<long>
{
   public required string Card { get; set; }
}
