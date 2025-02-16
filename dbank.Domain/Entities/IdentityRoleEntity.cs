using Microsoft.AspNetCore.Identity;

namespace DBank.Domain.Entities;

public class IdentityRoleEntity : IdentityRole<long>
{
    public CustomerEntity? Customer { get; set; }
}
